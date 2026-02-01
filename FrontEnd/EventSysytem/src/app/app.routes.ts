import { Routes } from '@angular/router';
import {  HomeComponent } from './Components/home/home';
import {  OrganizerComponent } from './Components/organizer/organizer';
import { Attendee } from './Services/attendee';
import { EventComponent } from './Components/event/event';
import { AttendeeComponent } from './Components/attendee/attendee';

export const routes: Routes = [
    {path:'home',component:HomeComponent},
    {path:'organizers',component:OrganizerComponent},
    {path:'attendees',component:AttendeeComponent},
    {path:'events',component:EventComponent},
    {path:'',redirectTo:'home',pathMatch:'full'}
    ,{path:'**',redirectTo:'home',pathMatch:'full'}
];
