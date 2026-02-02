import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EventParticipantsService {
  constructor(private _http:HttpClient) {}
  apiUrl:string="https://localhost:7087/api/ParticipantsInEvent";
  ///GetByCriteria

  getAllParticipantsInEvent(){

    return this._http.get(this.apiUrl+"/AllParticipantsInEvent");


  }
  getAllEventsInParticipant(){

    return this._http.get(this.apiUrl+"/AllEventsInParticipant");
  }

  GetAllParticipants(searchObj:any){
    let params :HttpParams=new HttpParams();
    params=params.set("ParticipantId",searchObj?.participantId??"");
    params=params.set("EventId",searchObj?.eventId??"");


    return this._http.get(this.apiUrl+"/GetByCriteria",{params:params});
  }
  
}
