import { Component } from '@angular/core';

import { Router, RouterLink } from "@angular/router";
import { Form, FormControl, FormControlName, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginServ } from '../../Services/login';
import { LoginInterface } from '../../Interface/LoginInterface';

@Component({
  selector: 'app-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  constructor(private _login:LoginServ,private router:Router){}

  Login: FormGroup = new FormGroup({
    userName: new FormControl<string | null>("", Validators.required),
    password: new FormControl<string | null>("", Validators.required)
  })

  login(){
    let loginVariable:LoginInterface={
      userName:this.Login.value.userName,
      password:this.Login.value.password
    }
   this._login.Login(loginVariable).subscribe({
       
    next:(res:any)=>{
      localStorage.setItem("Role",res.role);
      localStorage.setItem("Token",res.token);
      this.router.navigate(['/']);
      
      


    },
    error:err=>{
      alert(err.error.msg);



    }




   });

  }

}
