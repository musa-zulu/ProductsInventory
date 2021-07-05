import { Product } from './product';
export class Category {
    categoryId: string;
    name: string;
    categoryCode: string;
    isActive: boolean;
    dateCreated: Date;
    dateLastModified: Date;   
    createdBy: string;
    lastUpdatedBy: string;

    products: Product[];
}

