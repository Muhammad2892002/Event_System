import { Routes } from '@angular/router';
import {  HomeComponent } from './Components/home/home';

import { AttendeeService } from './Services/AttendeeService';
import { EventComponent } from './Components/event/event';
import { AttendeeComponent } from './Components/attendee/attendee';
import { EventParticipants } from './Components/event-participants/event-participants';
import { RequestToBeOrganizer } from './Components/request-to-be-organizer/request-to-be-organizer';
import { RequestsToJoinEvent } from './Components/requests-to-join-event/requests-to-join-event';

export const routes: Routes = [
    {path:'home',component:HomeComponent},
    {path:'attendees',component:AttendeeComponent},
    {path:'events',component:EventComponent},
    {path:'eventParticipants',component:EventParticipants},
    {path:'requestToBeOrganizer',component:RequestToBeOrganizer},
    {path:'requestsToJoinEvent',component:RequestsToJoinEvent},
    {path:'',redirectTo:'home',pathMatch:'full'}
    ,{path:'**',redirectTo:'home',pathMatch:'full'}
];
