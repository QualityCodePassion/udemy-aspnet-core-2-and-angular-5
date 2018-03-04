import { AuthService } from './../_services/auth.service';
import { AlertifyService } from './../_services/alertify.service';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/User';
import { Injectable } from '@angular/core';
import { UserService } from '../_services/user.service';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';

@Injectable()
export class MemberEditResolver implements Resolve<User> {

    constructor (private userService: UserService,
        private authService: AuthService,
        private router: Router,
        private alertify: AlertifyService) {}

    resolve (route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(this.authService.getUserId()).catch(error => {
            this.alertify.error('Problem retrieving the user');
            this.router.navigate(['/members/edit']);
            return Observable.of(null);
        });
    }
}
