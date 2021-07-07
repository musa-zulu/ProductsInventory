import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AccountService } from "src/app/shared/services/account.service";
import { Register } from "../../../models/register";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"],
})
export class RegisterComponent implements OnInit {
  registerModel: Register = new Register();
  isInvalid: boolean = false;
  constructor(private _accountService: AccountService,private _router: Router) {}

  ngOnInit() {}

  register(): void {
    var pattern = new RegExp(
      "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[-+_!@#$%^&*.,?]).+$"
    );

    if (pattern.test(this.registerModel.password)) {
      this.isInvalid = false;
      let registered = this._accountService.registerUser(
        this.registerModel
      );
      if (registered) {
        localStorage.removeItem("userDetails");
        localStorage.setItem("userDetails", JSON.stringify(this.registerModel.email));      
        this._router.navigate(['/dashbord']);
      } else {
        this.isInvalid = true;
      }
    } else {
      this.isInvalid = true;
    }
  }
}
