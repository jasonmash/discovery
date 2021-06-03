import { Part } from "./Part";

export interface DefectCategory {
  id: number;
  part?: Part;
  partId: number;
  title: string;
  description: string;
}