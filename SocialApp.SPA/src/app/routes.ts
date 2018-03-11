import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { AuthGuard } from './_guards/auth.guard';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';

export const appRoutes: Routes = [
{path: 'home', component: HomeComponent},
{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    // Add all routes that need to authorization to the array of children here:
    children: [
        {path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
        {path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver}},
        {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
                canDeactivate: [PreventUnsavedChanges]},
        {path: 'messages', component: MessagesComponent},
        {path: 'lists', component: ListsComponent}
    ]
},
// This is the "catchall" that will redirect to the home
{path: '**', redirectTo: 'home', pathMatch: 'full'}
];
