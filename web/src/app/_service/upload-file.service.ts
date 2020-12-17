import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { CommonSearchView } from '../_model/common-search-view';
import { UploadFileSearchView, UploadFileView } from '../_model/upload-file';

@Injectable({
  providedIn: 'root'
})
export class UploadFileService {

  constructor(
    private http: HttpClient
  ) { }

  public async searchUploadFile(_model: UploadFileSearchView) {
    return await this.http.post<CommonSearchView<UploadFileView>>(environment.API_URL + 'upload-file/postSearchUploadFile', _model).toPromise();
  }

  public async postUploadFileAdd(_model: UploadFileView) {
    var fd = new FormData();
    fd.append('pddsgn_code', _model.pddsgn_code);
    fd.append('type',_model.type);
    fd.append('file_name',_model.file_name);
    fd.append('file_path',_model.file_path);
    fd.append('file', _model.file);
    
    
    return await this.http.post<number>(environment.API_URL + 'upload-file/postCreate', fd).toPromise();
    
  }

  public async postUploadFileEdit(_model: UploadFileView) {
    var fd = new FormData();
    fd.append('pddsgn_code', _model.pddsgn_code);
    fd.append('type',_model.type);
    fd.append('file_name',_model.file_name);
    fd.append('file_path',_model.file_path);
    fd.append('file', _model.file);
    
    
    return await this.http.post<number>(environment.API_URL + 'upload-file/postUpdate', fd).toPromise();
    
  }

  public async getInfo(_code: string ) {
    return await this.http.get<UploadFileView>(environment.API_URL + 'upload-file/getInfo/' + _code).toPromise();
  }

  public async delete(params) {
    return await this.http.post(environment.API_URL + 'upload-file/post/Delete',params).toPromise();
  }
}
