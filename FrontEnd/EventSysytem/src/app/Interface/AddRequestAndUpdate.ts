export interface AddAndUpdateRequest{

    id:number|null;
    EventId:number;
    UserId:number;
    status?:number|null;
    RequestedAt?:string|null;

}