import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class RequestToJoinEventService {
  apiurl:string="https://localhost:7087/api/RequestToJoinEvent";
  constructor(private _http:HttpClient) {}

  GetAllAtendeeRequests(){

   return this._http.get(this.apiurl+"/GetAllAttendeesWithRequests");
  }
  GetAllEventsWithRequests(){
    return this._http.get(this.apiurl+"/GetAllEventsWithRequests");
  }

  GetAllRequest(searchObj:any){
    let params=new HttpParams();
    params=params.set("EventId",searchObj?.eventId??"");
    params=params.set("UserId",searchObj?.userId??"");
    params=params.set("Status",searchObj?.status??"");
    params=params.set("RequestedAt",searchObj?.requestedAt??"");  
    return this._http.get(this.apiurl+"/GetByCriteria",{params:params});
  }

  
  
}
