import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class Lookup {
  apiUr:string="https://localhost:7087/api/Lookups";
  constructor(private _http: HttpClient) {}


  GetAllPlaces(){

    return this._http.get(this.apiUr+'/GetAllPlaces');
  }

  
}
