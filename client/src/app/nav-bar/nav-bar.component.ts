import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AccountService } from "../shared/services/account.service";

@Component({
  selector: "app-nav-bar",
  templateUrl: "./nav-bar.component.html",
  styles: ['./nav-bar.component.css'],
})
export class NavBarComponent implements OnInit {
  userDetails: string;
  constructor(private _accountService: AccountService, private _router: Router) {   
  }

  ngOnInit() {
    var obj = JSON.parse(localStorage.getItem("userDetails"));
    this.userDetails =obj['email'];
  }
    
  logout() {    
    this._accountService.logout().then(() => {
      localStorage.removeItem("userDetails"); 
      this._router.navigate(["/"]);
      this.userDetails = null;
      location.reload();
    
    }).catch(err => {
      console.log(err);
    });    
  }
}
