import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanApproveView } from '../../_model/scan-approve-send';
import { ScanReceiveSearchView, ScanReceiveView, ScanReceiveFinView } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';

@Component({
  selector: 'app-scan-receive-create',
  templateUrl: './scan-receive-create.component.html',
  styleUrls: ['./scan-receive-create.component.scss']
})
export class ScanReceiveCreateComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ScanReceiveSearchView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
    ) { }

    public validationForm: FormGroup;
    public user: any;
    public model: ScanReceiveView = new ScanReceiveView();
    public searchModel: ScanReceiveSearchView = new ScanReceiveSearchView();
    public model_scan: ScanReceiveFinView = new ScanReceiveFinView();  
    public datas: any = {};
    public total = 0;
  
       
    ngOnInit() {
      this.buildForm();
      this.user = this._authSvc.getLoginUser();
      this.searchModel.doc_no = this.data.doc_no;
      this.searchModel.entity = this.data.entity;
      this.searchModel.scan_type = "SETNO";
    }
  
    private buildForm() {
      this.validationForm = this._fb.group({
        scan_data: [null, [Validators.required]],
        scan_qty: [null, [Validators.required]]
      });
    }
    
    async onQrEntered(_scan_data: string) {

      if (_scan_data == null || _scan_data == "") {
        return;
      }

      var datePipe = new DatePipe("en-US");
      this.searchModel.user_id = this.user.username;
      this.searchModel.build_type = this.user.branch.entity_code;
      
      
      console.log(this.searchModel);
      
      this.datas = await this._scanRecSvc.ScanReceiveNew(this.searchModel);
      console.log(this.datas)
      //this.model.doc_no = this.datas.doc_no;
      this.add(this.datas);
      
      
      this.searchModel.scan_data = "";

    }
  
    async save() {
  
      console.log(this.data);
      var datePipe = new DatePipe("en-US");
      this.searchModel.user_id = this.user.username;
      this.searchModel.build_type = this.user.branch.entity_code;
      
      
      console.log(this.searchModel);
      
      this.datas = await this._scanRecSvc.ScanReceiveNew(this.searchModel);
      console.log(this.datas)
      //this.model.doc_no = this.datas.doc_no;
      this.add(this.datas);
      
      
      this.searchModel.scan_data = "";
      this.searchModel.scan_qty = 1;
  
    }
  
    add(datas: any) {
  
      let newProd: ScanReceiveView = new ScanReceiveView();
      newProd.set_no = datas.set_no;
      newProd.prod_code = datas.prod_code;
      newProd.line_no = datas.line_no;
      newProd.qty = datas.qty;
      newProd.doc_no = datas.doc_no,
      newProd.entity = datas.entity
      
  
      this.total = this.total + datas.qty;
  
      this.model_scan.datas.push(newProd);
  
      
  
    }
    
    radioTypeChange(value) {
      this.searchModel.scan_type = value;
      
      console.log(this.searchModel.scan_type);
    }
  
    cancel(_index,scan)
    {
      console.log(scan); 

      this._msgSvc.confirmPopup("ยืนยันยกเลิกบันทึกรับมอบ", async result => {
        if (result) {
          let res: any = await this._scanRecSvc.ScanReceiveCancel(scan);
  
          this._msgSvc.successPopup(res.message);
          this.model_scan.datas.splice(_index, 1);
  
        }
      })
    }


  
    close()
      {
        this.dialogRef.close([]);
      }

}
