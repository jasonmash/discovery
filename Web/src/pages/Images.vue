<template>
  <k-list-details :show-details="showDetails">
    <template v-slot:list>
      <h2>Images</h2>
      <ul class="table">
        <li v-for="image in images" :key="image.id">
          <router-link :to="`/images/${image.id}`">{{image.timestamp}}</router-link>
        </li>
      </ul>
    </template>
    <template v-slot:detail>
      <div v-if="selectedImage">
        <h1><small>#{{selectedImage.id}}</small></h1>
        <p><img style="display:block; height:300px;" :src="`data:image/jpeg;base64,${selectedImage.imageData}`"/></p>
        <k-button variant="danger" @click="deleteImage">Delete</k-button>
      </div>
    </template>
  </k-list-details>
</template>

<script lang="ts">
import { KListDetails, KButton, KForm, KInput } from '../components';
import { defineComponent, ref, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { Image } from '../models/Image';
import { Part } from '../models/Part';

interface ImagesData {
  images: Part[],
  selectedImage?: Part
}

export default defineComponent({
  name: 'Images',
  components: { KListDetails, KButton, KForm, KInput },
  setup() {
    const route = useRoute();
    const router = useRouter();
    const selectedId = ref(route.params.id);
    const showDetails = ref(!!selectedId.value);
    const images = ref([] as Image[]);
    const selectedImage = ref();
    const showForm = ref(false);
    const newImage = ref({ title: "", description: ""});

    const loadData = () => {
      fetch('/api/images')
        .then(res => res.json())
        .then(res => {
          images.value = res;
          if (!!selectedId.value) {
            selectedImage.value = images.value.find((p: Image) => p.id.toString() == selectedId.value);
            fetch('/api/images/' + selectedId.value)
              .then(res => res.json())
              .then(res => {
                selectedImage.value = res;
              });
          }
        });
    };
    loadData();

    // fetch the user information when params change
    watch(() => route.params.id,
      async newId => {
        showForm.value = false;
        selectedId.value = newId;
        showDetails.value = !!selectedId.value;
        selectedImage.value = images.value.find(p => p.id.toString() == selectedId.value);
        
        fetch('/api/images/' + newId)
          .then(res => res.json())
          .then(res => {
            selectedImage.value = res;
          });
      }
    );

    var deleteImage = () => {
      fetch('/api/images/' + selectedId.value, {
        method: 'DELETE',
      })
      .then(res => {
        if (res.ok) {
          images.value.splice(images.value.findIndex(i => i.id.toString() == selectedId.value), 1);
          router.replace("/images");
        }
      });
    };

    return {
      selectedId,
      showDetails,
      images,
      selectedImage,
      showForm,
      newImage,
      deleteImage
    }
  }
})
</script>

