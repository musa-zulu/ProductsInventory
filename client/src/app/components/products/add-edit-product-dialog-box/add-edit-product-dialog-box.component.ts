import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Product } from 'src/app/shared/models/product';
import { CategoryService } from 'src/app/shared/services/category.service';

@Component({
  selector: 'app-add-edit-product-dialog-box',
  templateUrl: './add-edit-product-dialog-box.component.html',
  styleUrls: ['./add-edit-product-dialog-box.component.css']
})
export class AddEditProductDialogBoxComponent {

  action: string;
  localData: any;
  categories$;
  product: Product = new Product();

  constructor(
    private _categoryService: CategoryService,
    public dialogRef: MatDialogRef<AddEditProductDialogBoxComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Product) {
    console.log(data);
    this.localData = {...data};
    this.action = this.localData.action;

    this._categoryService.getCategories().subscribe((categories) => {
      this.categories$ = categories.data;
      console.log();
    });
  }

  doAction() {
    this.dialogRef.close({event: this.action, data: this.localData});
  }

  closeDialog() {
    this.dialogRef.close({event: 'Cancel'});
  }
}
