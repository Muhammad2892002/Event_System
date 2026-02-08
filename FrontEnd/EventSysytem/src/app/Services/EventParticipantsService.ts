import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EventParticipant } from '../Interface/event-participant';
import { AddAndUpdateParticipant } from '../Interface/AddParticipant';

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

  AddNewParticipant(participantObj:AddAndUpdateParticipant){

    return this._http.post(this.apiUrl+"/AddNewParticipants",participantObj)


  }

  GetParticipant(Id:number){

    return this._http.get(this.apiUrl+`/GetparticipantById/${Id}`);

  }

    updateParticipant(participantObj:AddAndUpdateParticipant){

    return this._http.put(this.apiUrl+"/EditRequest",participantObj)


  }

      DeleteParticipant(id?:number|null){

    return this._http.delete(this.apiUrl+`/DeleteRequest/${id}`);
  
  
}
}
