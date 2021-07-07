import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { DashBordComponent } from './dash-bord/dash-bord.component';
import { AppRoutingModule } from './app-routing.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { HttpClientModule } from '@angular/common/http';
import { ProductCardComponent } from './shared/components/product-card/product-card.component';
import { CategoryService } from './shared/services/category.service';
import { ProductsService } from './shared/services/products.service';
import { RegisterComponent } from './shared/components/account/register/register.component';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './shared/components/account/login/login.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatPaginatorModule, MatSortModule, MatTableModule } from '@angular/material';
import { ProductFormComponent } from './components/products/product-form/product-form.component';
import { FileUploadComponent } from './shared/components/file-upload/file-upload.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { DialogBoxComponent } from './components/categories/dialog-box/dialog-box.component';
import { CustomFormsModule } from 'ng2-validation';
import { AlertComponent } from './shared/components/alert/alert.component';
import { CommonModule } from '@angular/common';
import { AlertService } from './shared/services/alert.service';
import { AddEditProductDialogBoxComponent } from './components/products/add-edit-product-dialog-box/add-edit-product-dialog-box.component';


@NgModule({
  declarations: [
    AppComponent,
    DashBordComponent,
    NavBarComponent,
    ProductCardComponent,
    RegisterComponent,
    LoginComponent,
    ProductFormComponent,
    FileUploadComponent,
    CategoriesComponent,
    DialogBoxComponent,
    AlertComponent,
    AddEditProductDialogBoxComponent
  ],
  imports: [
    FormsModule,
    CustomFormsModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    NgbModule,
    AngularFontAwesomeModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CommonModule,
    MatPaginatorModule,    
    MatSortModule
  ],
  entryComponents: [DialogBoxComponent, AddEditProductDialogBoxComponent],
  providers: [CategoryService, ProductsService, AlertService],
  bootstrap: [AppComponent]
})
export class AppModule { }
