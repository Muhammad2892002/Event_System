import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AttendeeService {
  constructor(private _http:HttpClient) {


  }
  apiUrl:string='https://localhost:7087/api/Attendee';

  getByCriteria(searchObj:any){
    let params=new HttpParams();
    params=params.set("AteendeeFirstName",searchObj?.FirstName??"");
    params=params.set("AteendeeLastName",searchObj?.LastName??"");
    params=params.set("Role",searchObj?.RoleId??"");
    params=params.set("Gender",searchObj?.GenderId??"");
    params=params.set("IsActivated",searchObj?.ISActivated??"");
    return this._http.get(this.apiUrl+'/GetByCriteria',{params:params});



  }
  
}
