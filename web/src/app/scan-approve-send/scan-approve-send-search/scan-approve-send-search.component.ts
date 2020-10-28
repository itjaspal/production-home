import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { ScanApproveSendView, ScanApproveSendSearchView } from '../../_model/scan-approve-send';
import { AuthenticationService } from '../../_service/authentication.service';
import { DropdownlistService } from '../../_service/dropdownlist.service';
import { MessageService } from '../../_service/message.service';
import { ScanApproveSendService } from '../../_service/scan-approve-send.service';
import { ScanApproveSendCreateComponent } from '../scan-approve-send-create/scan-approve-send-create.component';

@Component({
  selector: 'app-scan-approve-send-search',
  templateUrl: './scan-approve-send-search.component.html',
  styleUrls: ['./scan-approve-send-search.component.scss']
})
export class ScanApproveSendSearchComponent implements OnInit {

  constructor(
    private _fb: FormBuilder,
    private _authSvc: AuthenticationService,
    private _dialog: MatDialog,
    private _msgSvc: MessageService,
    private _router: Router,
    private _actRoute: ActivatedRoute,
    private _scanapvSendSvc: ScanApproveSendService,
    private cdr: ChangeDetectorRef,
    private _dll: DropdownlistService,
  ) { }

  public validationForm: FormGroup;
  public user: any;
  public model: ScanApproveSendView = new ScanApproveSendView();
  public searchModel: ScanApproveSendSearchView = new ScanApproveSendSearchView();

  public data: any = {};
  // public model_scan: ScanSendFinView = new ScanSendFinView();
  // public model_setno : PrintSetNoView = new PrintSetNoView();
  public datas: any = {};
  public datas_print: any = {};
  //public wclist: any; 

  public data_qty : any;
  
  async ngOnInit() {
    this.buildForm();
    this.user = this._authSvc.getLoginUser();
    // this.wclist = await this._dll.getDdlWCSend(this.user.username);
    // if(this.wclist.length==1)
    // {
    //   this.searchModel.wc_code = this.wclist[0].key;
    // }
    //this.searchModel.req_date = new Date()
    console.log(this.data);
    
  }

  private buildForm() {
    this.validationForm = this._fb.group({
      doc_no: [null, []],
      fin_date: [null, [Validators.required]],
      //send_type:[null, [Validators.required]]
    });
  }

  openScanApproveNew(p_entity : string ,p_req_date: string, p_wc_code: string, _index: number = -1)
  {
    const dialogRef = this._dialog.open(ScanApproveSendCreateComponent, {
      maxWidth: '100vw',
      maxHeight: '100vh',
      height: '100%',
      width: '100%',
      data: {
        tran_date: p_req_date,
        wc_code:p_wc_code,
        entity:p_entity
      }

    });

    dialogRef.afterClosed().subscribe(result => {
      if (result.length > 0) {
        //this.add_prod(result);
      }
    })
  }

  radioTypeChange(value) {
    this.searchModel.send_type = value;
    
    console.log(this.searchModel.send_type);
  }

  search()
  {

  }

  cancel(_index,scan)
  {

  }

  close() {
    this._router.navigateByUrl('/app/home');
  }

}
