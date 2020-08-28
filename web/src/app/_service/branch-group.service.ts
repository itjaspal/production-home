import { environment } from './../../environments/environment';
import { CommonSearchView } from './../_model/common-search-view';
import { BranchGroupSearchView, BranchGroupView } from './../_model/branchGroup';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BranchGroupService {

  constructor(private http: HttpClient) { }

  public async search(_model: BranchGroupSearchView) {
    return await this.http.post<CommonSearchView<BranchGroupView>>(environment.API_URL + 'master-branch-group/postSearch', _model).toPromise();
  }

  public async create(_model: BranchGroupView) {
    return await this.http.post<number>(environment.API_URL + 'master-branch-group/postCreate', _model).toPromise();
  }

  public async update(_model: BranchGroupView) {
    return await this.http.post<number>(environment.API_URL + 'master-branch-group/postUpdate', _model).toPromise();
  }

  public async getInfo(_id: number) {
    return await this.http.get<BranchGroupView>(environment.API_URL + 'master-branch-group/getInfo/' + _id).toPromise();
  }

}