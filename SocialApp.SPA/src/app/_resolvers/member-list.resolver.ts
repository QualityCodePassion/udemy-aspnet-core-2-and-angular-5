import { AlertifyService } from './../_services/alertify.service';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { UserService } from '../_services/user.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';
import { PaginationResult } from '../_models/pagination';

@Injectable()
export class MemberListResolver implements Resolve<PaginationResult<User[]>> {
    readonly page = 1;
    readonly pageSize = 5;

    constructor (private userService: UserService,
        private router: Router, private alertify: AlertifyService) {}

    resolve (route: ActivatedRouteSnapshot): Observable<PaginationResult<User[]>> {
        return this.userService.getUsers(this.page, this.pageSize).catch(error => {
            this.alertify.error('Problem retrieving the user');
            this.router.navigate(['/home']);
            return Observable.of(null);
        });
    }
}
