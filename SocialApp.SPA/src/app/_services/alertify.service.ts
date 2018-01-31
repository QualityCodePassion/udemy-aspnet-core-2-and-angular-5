import { Injectable } from '@angular/core';
declare let alertify: any;

@Injectable()
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: () => any) {
    alertify.confirm(message, function(e) {
        if (e) {
            okCallback();
        } else {
            console.log('Cancelled');
        }
    });
}

success(message: string) {
    alertify.success(message);
}

error(message: string) {
    alertify.error(message);
}

warning(message: string) {
    alertify.warning(message);
}

message(message: string) {
    alertify.message(message);
}

}
