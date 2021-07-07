import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from '../shared/models/category';
import { Product } from '../shared/models/product';
import { AccountService } from '../shared/services/account.service';
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
  products: Product[] = [];

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
        this.applyFilter(null);
       }
    });
  }

  private applyFilter(categoryId: string) {
    let category = [];
    this.products = [];
    if (categoryId !== null) {
      category = this.categories.filter(x => x.categoryId === categoryId);
      this.products = category[0].products;
    } else {
      (this.categories || []).forEach(cItem => {
        cItem.products.forEach(item => {
          if (this.products.indexOf(item) === -1) {
            item.imagePath = "https://localhost:5001/" + item.imagePath;
            this.products.push(item);
          }
        });
      });
    }
  }
}

