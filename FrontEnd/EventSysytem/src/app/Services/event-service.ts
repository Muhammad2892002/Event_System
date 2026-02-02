import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  constructor(private _http:HttpClient) {}
  apiUrl:string="https://localhost:7087/api/Event";

  getAllEvents(searchEvent:any){
    let params=new HttpParams();
    params=params.set("title",searchEvent?.title??"");
    params=params.set("eventDate",searchEvent?.eventDate??"");
    params=params.set("organizerId",searchEvent?.organizerId??"");
    params=params.set("createdAt",searchEvent?.createdAt??"");
    params=params.set("LocationId",searchEvent?.LocationId??"");


    return this._http.get(this.apiUrl+"/GetByCriteria",{params:params});

  }
  
}
