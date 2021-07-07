import { Component, OnInit, ViewChild } from "@angular/core";
import { ProductsService } from "src/app/shared/services/products.service";
import "rxjs/add/operator/take";
import { Product } from "src/app/shared/models/product";
import { MatDialog, MatTable, MatTableDataSource, PageEvent } from "@angular/material";
import { GetUserDto } from "src/app/shared/Dtos/get-user-dto";
import { AlertService } from "src/app/shared/services/alert.service";
import { AddEditProductDialogBoxComponent } from "../add-edit-product-dialog-box/add-edit-product-dialog-box.component";
import * as FileSaver from 'file-saver';

@Component({
  selector: "app-product-form",
  templateUrl: "./product-form.component.html",
  styleUrls: ["./product-form.component.css"],
  providers: [
    { provide: Window, useValue: window }
  ]
})

export class ProductFormComponent implements OnInit {
  products: Product[];
  product: Product = new Product();
  fileToUpload: File | null = null;
  pageLength: number = 0;

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  pageEvent: PageEvent;

  static readonly POLLING_INTERVAL = 4000;

  filteredProducts: Product[] = [];
  displayedColumns: string[] = [
    "name",
    "productCode",
    "description",
    "price",
    "action",
  ];
  tableDataResource = new MatTableDataSource<Product>();
  getUserDto: GetUserDto = new GetUserDto();
  userDetails: string;

  constructor(
    private _productService: ProductsService,
    public dialog: MatDialog,
    protected alertService: AlertService,
    private window: Window
  ) {

  }

  ngOnInit() {
    const userDetails = JSON.parse(localStorage.getItem("userDetails"));
    if (userDetails !== null && userDetails !== undefined) {
      this.getUserDto.email = userDetails["email"];
      this.getUserDto.userId = userDetails["userId"];
    }
    this.getProducts();
  }

  getProducts() {
    this._productService.getProducts().subscribe((products) => {
      this.products = products.data;
      console.log();      
      this.pageLength = this.products.length;
      this.onPageChanged(null);
    });
  }

  openDialog(action: any, product) {
    product.action = action;
    const dialogRef = this.dialog.open(AddEditProductDialogBoxComponent, {
      width: "450px",
      height: "600px",
      data: product,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (action === "Add") {
        this.addProduct(result.data);
      } else if (action === "Update") {
        this.updateProduct(result.data);
      } else if (action === "Delete") {
        this.deleteProduct(result.data);
      }
    });

    this.dialog.afterAllClosed.subscribe(() => {
      this.refreshTable();
    });
  }

  setProductInfo(product) {
    var imagePath = JSON.parse(localStorage.getItem("imagePath"));
    product.userId = this.getUserDto.userId;
    product.userName = this.getUserDto.email;
    if (imagePath !== null) {
      product.imagePath = "https://localhost:5001/" + imagePath["dbPath"];
      localStorage.removeItem("imagePath");
    } else {
      localStorage.removeItem("imagePath");
    }
  }

  async addProduct(product: Product) {
    this.setProductInfo(product);
    await this._productService
      .addProduct(product)
      .then((result) => {
        this.alertService.success("Product was saved successfully !!");
        this.getProducts();
      })
      .catch((error) => {
        var errorJson = JSON.stringify(error);
        var errorObject = JSON.parse(errorJson);
        var message = errorObject.error.errors[0].message;
        this.alertService.error(
          "Data was not saved due to the following reason:: " + message
        );
      });
    this.refreshTable();
  }

  async updateProduct(product: Product) {
    this.setProductInfo(product);
    this._productService
      .updateProduct(product)
      .then((result) => {
        this.alertService.success("Product was updated successfully !!");
        this.getProducts();
      })
      .catch((error) => {
        var errorJson = JSON.stringify(error);
        var errorObject = JSON.parse(errorJson);
        var message = errorObject.error.errors[0].message;
        this.alertService.error(
          "Data was not updated due to the following reason:: " + message
        );
      });
    this.refreshTable();
  }

  async deleteProduct(product: Product) {
    this._productService
      .deleteProduct(product)
      .then((result) => {
        this.alertService.success("Product was deleted successfully !!");
        this.getProducts();
      })
      .catch((error) => {
        var errorJson = JSON.stringify(error);
        var errorObject = JSON.parse(errorJson);
        var message = errorObject.error.errors[0].message;
        this.alertService.error(
          "Data was not deleted due to the following reason:: " + message
        );
      });
    this.refreshTable();
  }

  private initializeTable(products: Product[]) {
    this.tableDataResource = new MatTableDataSource<Product>(products);
  }

  filter(query: string) {
    const filteredProducts = query
      ? this.products.filter((p) =>
          p.name.toLowerCase().includes(query.toLowerCase())
        )
      : this.products;

    this.initializeTable(filteredProducts);
  }

  refreshTable() {
    this.getProducts();
  }
  
  onPageChanged(e) {
    let filteredProducts = [];
    if (e == null) {
      let firstCut = 0;
      let secondCut = firstCut + 10;
      filteredProducts = this.products.slice(firstCut, secondCut);
    } else {
      let firstCut = e.pageIndex * e.pageSize;
      let secondCut = firstCut + e.pageSize;
      filteredProducts = this.products.slice(firstCut, secondCut);
    }
    this.initializeTable(filteredProducts);
  }

  downloadExcel() {
    console.log();      
    this._productService.downloadExcel().subscribe(result => {  
    //  window.open(result);
    let a = document.getElementById('downloader');
    if (!a) {
       a = document.createElement('a');
       a.id = 'downloader';
       //a.target = '_blank'; 
       a.style.visibility = "hidden";
       document.body.appendChild(a);
    }
    //a.href = 'https://localhost:44396/api/ApprovalQuality/download?fileName=' + result;
    a.click();
    });
  }
}
