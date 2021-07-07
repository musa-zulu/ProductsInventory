import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CategoriesComponent } from './components/categories/categories.component';
import { ProductFormComponent } from './components/products/product-form/product-form.component';
import { DashBordComponent } from './dash-bord/dash-bord.component';
import { LoginComponent } from './shared/components/account/login/login.component';
import { RegisterComponent } from './shared/components/account/register/register.component';

const routes: Routes = [
  {path: '', redirectTo: '/dashbord', pathMatch: 'full'},
  {path: 'dashbord', component: DashBordComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent},
  {path: 'products', component: ProductFormComponent},
  {path: 'categories', component: CategoriesComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
