import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Attendee } from '../Interface/attendee';
import { LoginInterface } from '../Interface/LoginInterface';


@Injectable({
  providedIn: 'root',
})
export class LoginServ {

   constructor(private _http:HttpClient) {}
  apiUrl:string="https://localhost:7087/api/Auth";

  Login(login:LoginInterface){
    return this._http.post(this.apiUrl+"/Login",login);
  }

 
  
}
