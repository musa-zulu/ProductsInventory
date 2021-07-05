import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashBordComponent } from './dash-bord/dash-bord.component';

const routes: Routes = [
  {path: '', redirectTo: '/dashbord', pathMatch: 'full'},
  {path: 'dashbord', component: DashBordComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
