export interface EventRequest{
id:number;
eventId:number;
userId:number;
eventName?:string |null;
userName?:string |null;
email?:string |null;
status?:number |null;
statusName?:string |null;
requestedAt?:string |null;

}