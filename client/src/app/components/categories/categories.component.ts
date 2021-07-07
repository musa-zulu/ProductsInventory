import { Component, OnInit, ViewChild } from "@angular/core";
import { MatDialog, MatTable, MatTableDataSource } from "@angular/material";
import { timer } from "rxjs/internal/observable/timer";
import { switchMap } from "rxjs/operators";
import { GetUserDto } from "src/app/shared/Dtos/get-user-dto";
import { Category } from "src/app/shared/models/category";
import { AccountService } from "src/app/shared/services/account.service";
import { AlertService } from "src/app/shared/services/alert.service";
import { CategoryService } from "src/app/shared/services/category.service";
import { DialogBoxComponent } from "./dialog-box/dialog-box.component";

@Component({
  selector: "app-categories",
  templateUrl: "./categories.component.html",
  styleUrls: ["./categories.component.css"],
})
export class CategoriesComponent implements OnInit {
  constructor(
    private _categoryService: CategoryService,
    public dialog: MatDialog,
    protected alertService: AlertService,
    private _accountService: AccountService
  ) {}

  static readonly POLLING_INTERVAL = 4000;
  categories: Category[];
  filteredCategories: Category[] = [];
  displayedColumns: string[] = ["name", "code", "action"];
  tableDataResource = new MatTableDataSource<Category>();
  getUserDto: GetUserDto = new GetUserDto();
  userDetails: string;

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;

  async ngOnInit() {       
    const userDetails = JSON.parse(localStorage.getItem('userDetails'));    
    if (userDetails !== null && userDetails !== undefined) {
      this.getUserDto.email = userDetails['email'];
      this.getUserDto.userId = userDetails['userId'];
    }
    
    this.getCategories().subscribe((categories) => {
      this.categories = categories.data;
      this.initializeTable(this.categories);
      console.log();
    });
  }

  setCategoryInfo(category){    
    category.userId = this.getUserDto.userId;
    category.userName = this.getUserDto.email;    
  }

  openDialog(action: any, category) {
    category.action = action;
    const dialogRef = this.dialog.open(DialogBoxComponent, {
      width: "300px",
      height: "400px",
      data: category,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (action === "Add") {
        this.addCategory(result.data);
      } else if (action === "Update") {
        this.updateCategory(result.data);
      } else if (action === "Delete") {
        this.deleteCategory(result.data);
      }
    });

    this.dialog.afterAllClosed.subscribe(() => {
      this.refreshTable();
    });
  }

  getCategories() {
    return timer(0, CategoriesComponent.POLLING_INTERVAL).pipe(
      switchMap(() => this._categoryService.getCategories())
    );
  }

  async addCategory(category: Category) {
    this.setCategoryInfo(category);
    await this._categoryService
      .addCategory(category)
      .then((result) => {
        this.alertService.success("Category was saved successfully !!");
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

  updateCategory(category: Category) {
    this.setCategoryInfo(category);
    this._categoryService
      .updateCategory(category)
      .then((result) => {
        this.alertService.success("Category was updated successfully !!");
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

  deleteCategory(category: Category) {
    this._categoryService
      .deleteFoodCategory(category.categoryId)
      .then((result) => {
        this.alertService.success("Category was deleted successfully !!");
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

  private initializeTable(categories: Category[]) {
    this.tableDataResource = new MatTableDataSource<Category>(categories);
  }

  filter(query: string) {
    const filteredFoodCategories = query
      ? this.categories.filter((p) =>
          p.name.toLowerCase().includes(query.toLowerCase())
        )
      : this.categories;

    this.initializeTable(filteredFoodCategories);
  }

  refreshTable() {
    this.getCategories();
  }
}
