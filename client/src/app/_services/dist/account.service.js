"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
exports.__esModule = true;
exports.AccountService = void 0;
var core_1 = require("@angular/core");
var rxjs_1 = require("rxjs");
var operators_1 = require("rxjs/operators");
var environment_1 = require("src/environments/environment");
var AccountService = /** @class */ (function () {
    function AccountService(http) {
        this.http = http;
        this.baseUrl = environment_1.environment.apiUrl;
        // is a buffer object
        // how many users are we going to store, the value is one
        // the current user object
        // is like a buffer (stores the value)
        this.currentUserSource = new rxjs_1.ReplaySubject(1);
        // by convention since it is an observable we give it $ at the end
        this.currentUser$ = this.currentUserSource.asObservable();
    }
    AccountService.prototype.login = function (model) {
        var _this = this;
        return this.http.post(this.baseUrl + 'account/login', model).pipe(operators_1.map(function (response) {
            var user = response;
            if (user) {
                localStorage.setItem('user', JSON.stringify(user));
                // to set the next value
                _this.currentUserSource.next(user);
            }
        }));
    };
    AccountService.prototype.register = function (model) {
        var _this = this;
        return this.http.post(this.baseUrl + 'account/register', model).pipe(operators_1.map(function (user) {
            if (user) {
                localStorage.setItem('user', JSON.stringify(user));
                _this.currentUserSource.next(user);
            }
        }));
    };
    // adding a helper method
    AccountService.prototype.setCurrentUser = function (user) {
        this.currentUserSource.next(user);
    };
    AccountService.prototype.logout = function () {
        localStorage.removeItem('user');
        this.currentUserSource.next(null);
    };
    AccountService = __decorate([
        core_1.Injectable({
            providedIn: 'root'
        })
    ], AccountService);
    return AccountService;
}());
exports.AccountService = AccountService;
