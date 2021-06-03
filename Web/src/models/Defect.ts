import { DefectCategory } from "./DefectCategory";
import { Part } from "./Part";

export interface Defect {
  id: number;
  part?: Part;
  partId: number;
  category?: DefectCategory;
  categoryId: number;
  title: string;
  description: string;
}