import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadFileView } from '../../_model/upload-file';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { UploadFileService } from '../../_service/upload-file.service';

@Component({
  selector: 'app-upload-file-add',
  templateUrl: './upload-file-add.component.html',
  styleUrls: ['./upload-file-add.component.scss']
})
export class UploadFileAddComponent implements OnInit {

  constructor(
    private _formBuilder: FormBuilder,
    private _actRoute:ActivatedRoute,
    private _uploadSvc: UploadFileService,
    private _msgSvc: MessageService,
    private _router: Router,
    private _dll: DropdownlistService,
  ) { }

  public model: UploadFileView = new UploadFileView();
  public validationForm: FormGroup;
  selectedFiles: FileList;
  fileName: any;
  public deptlist: any; 

  async ngOnInit() {
    this.buildForm();
    this.deptlist = await this._dll.getDdlDeptMarketing();
  }
  buildForm() {
    this.validationForm = this._formBuilder.group({
      pddsgn_code: [null, [Validators.required]],
      type: [null, [Validators.required]],
      dsgn_no :[null, [Validators.required]],
      dept_code :[null, [Validators.required]],
    });
  }

  fileChange(event) {
    this.selectedFiles = event.target.files;
    this.selectedFiles = event.target.files;
    this.fileName = this.selectedFiles[0].name;
    //console.log('selectedFiles: ' + this.fileName );
    if (this.selectedFiles.length > 0) {
      this.model.file = this.selectedFiles[0];
    } else {
      this.model.file = null;
    }
  
   
    console.log(this.model.file);
  }

  close() {
    this._router.navigateByUrl('/app/upload'); 
  }

  async save() {

    
    // this.model.co_trns_mast_id = this._actRoute.snapshot.params.id;
    this.model.file_path = this.model.file.name;
    this.model.file_name = this.model.file.name;

    console.log(this.model);

    if(this.model.pddsgn_code == "")
    {
      await this._msgSvc.warningPopup("ต้องใส่ข้อมูล");
    }
    else
    {
      await this._uploadSvc.postUploadFileAdd(this.model);

      await this._msgSvc.successPopup("บันทึกข้อมูลเรียบร้อย");
      this._router.navigateByUrl('/app/upload'); 
    }
    console.log(this.model.file);

  }

}
