import { environment } from './../../environments/environment';
import { CommonSearchView } from './../_model/common-search-view';
import { BranchSearchView, BranchView } from './../_model/branch';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {

  constructor(private http: HttpClient) { }

  public async searchBranch(_model: BranchSearchView) {
    return await this.http.post<CommonSearchView<BranchView>>(environment.API_URL + 'organization/postSearchBranch', _model).toPromise();
  }

  public async saveBranch(_model: BranchView) {
    return await this.http.post<number>(environment.API_URL + 'organization/postSaveBranch', _model).toPromise();
  }

  public async getBranchInfo(_branchId: number) {
    return await this.http.get<BranchView>(environment.API_URL + 'organization/getBranchInfo/' + _branchId).toPromise();
  }

  public async inquiryBranchs() {
    return await this.http.get(environment.API_URL + 'organization/branchGroups').toPromise();
  }

  public async searchBranchById(_model: BranchSearchView) {
    return await this.http.post<CommonSearchView<BranchView>>(environment.API_URL + 'organization/postSearchBranchById', _model).toPromise();
  }
}
