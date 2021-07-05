import { Component, OnInit } from '@angular/core';
import { Category } from '../shared/models/category';
import { Product } from '../shared/models/product';
import { CategoryService } from '../shared/services/category.service';

@Component({
  selector: 'app-dash-bord',
  templateUrl: './dash-bord.component.html',
  styles: ['./dash-bord.component.css']
})
export class DashBordComponent implements OnInit {

  currentCategory: string;
  categories: Category[];
  category: Category;
  executed = false;
  filteredProducts: Product[] = [];

  constructor(private _categoryService: CategoryService) { }

  ngOnInit() {
    this.getCategories();
    this.setCurrentCategory(null);
  }

  setCurrentCategory(newCategory: string) {
    this.currentCategory = newCategory;
    if (newCategory === null)   {
       this.getCategories();
    }
    this.applyFilter(newCategory);
  }

  getCategories() {
    this._categoryService.getCategories()
    .subscribe(async (categories) => {
       this.categories = await categories.data;       
       if (!this.executed) {
        this.executed = true;
        this.populateProductsFor(this.categories);
       }
    });
  }

  populateProductsFor(categories: Category[]) {
    categories.forEach(element => {
      element.products.forEach(fi => {
        if (this.filteredProducts.indexOf(fi) === -1) {
          this.filteredProducts.push(fi);
        }
      });
    });
  }

  applyFilter(categoryId: string) {
    let category = [];
    
    if (categoryId !== null) {
      category = this.categories.filter(x => x.categoryId === categoryId);
      
    } else {
      (this.categories || []).forEach(cItem => {
        
        
      });
    }
  }
}
