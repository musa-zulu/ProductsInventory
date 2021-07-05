import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Login } from "src/app/shared/models/login";
import { AccountService } from "src/app/shared/services/account.service";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  loginModel: Login = new Login();
  isInvalid: boolean = false;
  constructor(
    private _accountService: AccountService,
    private _router: Router
  ) {}

  ngOnInit() {}

  login() {
    this.isInvalid = false;
    this._accountService.login(this.loginModel).then(() => {      
      this._router.navigate(["/dashbord"]);
    }).catch(err => {
      this.isInvalid = true;
    });    
  }
}
