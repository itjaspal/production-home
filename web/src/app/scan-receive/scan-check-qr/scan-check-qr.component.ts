import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanCheckQrSearchView, ScanCheckQrView, ScanCheckQrFinView } from '../../_model/scan-receive';
import { AuthenticationService } from '../../_service/authentication.service';
import { MessageService } from '../../_service/message.service';
import { ScanReceiveService } from '../../_service/scan-receive.service';

@Component({
  selector: 'app-scan-check-qr',
  templateUrl: './scan-check-qr.component.html',
  styleUrls: ['./scan-check-qr.component.scss']
})
export class ScanCheckQrComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ScanCheckQrSearchView,
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanRecSvc: ScanReceiveService,
    private cdr: ChangeDetectorRef,
   
  ) { }

  public validationForm: FormGroup;
    public user: any;
    public model: ScanCheckQrView = new ScanCheckQrView();
    public searchModel: ScanCheckQrSearchView = new ScanCheckQrSearchView();
    public model_scan: ScanCheckQrFinView = new ScanCheckQrFinView();  
    public datas: any = {};
    public total = 0;
  
       
    ngOnInit() {
      this.buildForm();
      this.user = this._authSvc.getLoginUser();
      
    }
  
    private buildForm() {
      this.validationForm = this._fb.group({
        qr: [null, [Validators.required]]
      });
    }
    
    async onQrEntered(_qr: string) {

      if (_qr == null || _qr == "") {
        return;
      }

          
      
      
      console.log(this.searchModel);
      
      this.datas = await this._scanRecSvc.ScanCheckQr(this.searchModel);
      console.log(this.datas)
      //this.model.doc_no = this.datas.doc_no;
      this.add(this.datas);
      
      
      this.searchModel.qr = "";

    }

    add(datas: any) {
  
      let newProd: ScanCheckQrView = new ScanCheckQrView();
      newProd.prod_code = datas.prod_code;
      newProd.prod_name = datas.prod_name;
      newProd.design_name = datas.design_name;
      newProd.req_date = datas.req_date;
      newProd.doc_no = datas.doc_no,
      newProd.doc_date = datas.doc_date,
      newProd.set_no = datas.set_no,
  
      
      this.model_scan.datas.push(newProd);
  
      // Group By Product
    var result = [];
    this.model_scan.datas.forEach(function (a) {
      if (!this[a.set_no] ) {
        this[a.set_no] = { set_no: a.set_no , doc_no : a.doc_no, doc_date: a.doc_date};
        result.push(this[a.set_no]);
        
      }
      //this[a.prod_code].qty += a.qty;

    }, Object.create(null));

    this.model_scan.datas = result.sort((a, b) => (a.set_no > b.set_no) ? 1 : (a.set_no === b.set_no) ? ((a.doc_no > b.doc_no) ? 1 : -1) : -1);
    console.log(result);
  
    }

    close()
    {
      this.dialogRef.close([]);
    }

}
