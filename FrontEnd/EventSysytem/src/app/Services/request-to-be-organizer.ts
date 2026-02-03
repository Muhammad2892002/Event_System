import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RequestToBeOrganizerService {
  constructor(private _http:HttpClient){}
  apiUrl:string="https://localhost:7087/api/RequestToBeAnOrganizer";

  GetAllRequests(searchObj:any){
    let params=new HttpParams();
    params=params.set("status",searchObj?.status??"");
    params=params.set("requestedTime",searchObj?.requestedTime??"");


    return this._http.get(this.apiUrl+"/GetByCriteria",{params:params});
  }

  
}
