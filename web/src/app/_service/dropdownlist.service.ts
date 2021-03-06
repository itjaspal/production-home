import { Dropdownlist } from './../_model/dropdownlist';
import { environment } from './../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DropdownlistService {

  constructor(private http: HttpClient) { } 


  public async getDdlDefaultPrinterJIT() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlMobilePrntJIT').toPromise();
  }

  public async getDdlDefaultPrinterSTK() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlMobilePrntSTK').toPromise();
  }

  public async getDdlBranchStatus() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchStatus').toPromise();
  }

  public async getDdlBranchGroup() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchGroup').toPromise();
  }

  public async getDdlBranch(branchId: number = 0) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranch/' + branchId).toPromise();
  }

  public async getDdlTransferBranch(branchId: number = 0) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlTransferBranch/' + branchId).toPromise();
  }

  public async getDdlBranchInGroup(branchGroupId) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchInGroup/' + branchGroupId).toPromise();
  }

  public async getDdlBranchInGroupRpt(branchGroupId) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlBranchInGroupRpt/' + branchGroupId).toPromise();
  }

  public async getDdlDepartments() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlDepartment').toPromise();
  }

  public async getDdlUserRoles(_isPC, _createUser) {
        
    var ownerRole : any = {isPc : _isPC, createUser: _createUser };

    return await this.http.post(environment.API_URL + 'dropdownlist/inquiryDdlUserRole', ownerRole).toPromise();
  }

  public async getDdlWCInprocess(_user: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlWCInprocess/' + _user).toPromise();
  }

  public async getDdlWCSend(_user: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlWCSend/' + _user).toPromise();
  }

  public async getDdlWCPtwByUser(_entity: string, _user: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlWCPtwByUser/' + _entity + '/' + _user).toPromise();
  }

  public async getDdlPutAwayWHMast() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlPutAwayWHMast').toPromise();
  }

  public async getDdlPtwSetNoList(_entity: string, _docCode: string, _docNo: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlPtwSetNoList/' + _entity + '/' + _docCode + '/' + _docNo).toPromise();
  }

  public async getDdlPtwProdList(_entity: string, _docCode: string, _docNo: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlPtwProdList/' + _entity + '/' + _docCode + '/' + _docNo).toPromise();
  }

  public async getDdlWCInprocessStock(_user: string) {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlWCInprocessStock/' + _user).toPromise();
  }
  
  public async getDdlDeptMarketing() {
    return await this.http.get<Dropdownlist[]>(environment.API_URL + 'dropdownlist/getDdlDeptMarketing').toPromise();
  }
}
