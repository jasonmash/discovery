import { ref, watch } from "vue";

export default function (startVal: string, validators: Function[], onValidate: Function) {
  const input = ref(startVal);
  const errors = ref([] as string[]);
  watch(input, value => {
    errors.value = validators.map(validator => validator(value));
    onValidate(value);
  });
  return {
    input,
    errors
  }
}