import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { showAllOragnizerRequest } from '../Interface/show-organizers-request';

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

  UpdateRequest(obj:showAllOragnizerRequest){
    let request:any={
      requestId:obj.id,
      status:obj.statusId

    }

    return this._http.put(this.apiUrl+"/UpdateRequest",request);
  }


  
      DeleteRequestToBeAnOrgainizer(id?:number |null){

    return this._http.delete(this.apiUrl+`/DeleteRequest/${id}`);
  
  
}

  
}
