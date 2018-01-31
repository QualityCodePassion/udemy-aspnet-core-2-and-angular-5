import { AuthGuard } from './_guards/auth.guard';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './member-list/member-list.component';
import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';

export const appRoutes: Routes = [
{path: 'home', component: HomeComponent},
{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    // Add all routes that need to authorization to the array of children here:
    children: [
        {path: 'members', component: MemberListComponent},
        {path: 'messages', component: MessagesComponent},
        {path: 'lists', component: ListsComponent}
    ]
},
// This is the "catchall" that will redirect to the home
{path: '**', redirectTo: 'home', pathMatch: 'full'}
];
