VERSION 5.00
Object = "{C932BA88-4374-101B-A56C-00AA003668DC}#1.1#0"; "MSMASK32.OCX"
Begin VB.Form frmCMS1500 
   Caption         =   "Form1"
   ClientHeight    =   10320
   ClientLeft      =   165
   ClientTop       =   450
   ClientWidth     =   15915
   LinkTopic       =   "Form1"
   ScaleHeight     =   10320
   ScaleWidth      =   15915
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox Picture1 
      Height          =   13410
      Left            =   600
      ScaleHeight     =   13350
      ScaleWidth      =   13995
      TabIndex        =   0
      Top             =   -120
      Width           =   14055
      Begin VB.PictureBox pbxClaim 
         BackColor       =   &H8000000B&
         Height          =   16000
         Left            =   2160
         ScaleHeight     =   15945
         ScaleWidth      =   11715
         TabIndex        =   1
         Top             =   240
         Width           =   11775
         Begin VB.TextBox txtOthInsdMI 
            Height          =   285
            Left            =   3960
            TabIndex        =   87
            Top             =   2580
            Width           =   375
         End
         Begin VB.TextBox txtOthInsdFirstName 
            Height          =   285
            Left            =   2160
            TabIndex        =   86
            Top             =   2580
            Width           =   1695
         End
         Begin VB.TextBox txtInsuredMI 
            Height          =   285
            Left            =   11040
            TabIndex        =   85
            Top             =   660
            Width           =   375
         End
         Begin VB.TextBox txtInsuredFirstName 
            Height          =   285
            Left            =   9720
            TabIndex        =   84
            Top             =   660
            Width           =   1215
         End
         Begin VB.TextBox txtPatientMI 
            Height          =   285
            Left            =   3960
            TabIndex        =   83
            Top             =   660
            Width           =   375
         End
         Begin VB.TextBox txtPatientFirstName 
            Height          =   285
            Left            =   2160
            TabIndex        =   82
            Top             =   660
            Width           =   1695
         End
         Begin VB.TextBox txt32a 
            Height          =   285
            Left            =   4080
            TabIndex        =   81
            Top             =   12720
            Width           =   1695
         End
         Begin VB.TextBox txt32b 
            Height          =   285
            Left            =   6000
            TabIndex        =   80
            Top             =   12720
            Width           =   1575
         End
         Begin VB.TextBox txtLine1DX 
            Height          =   285
            Left            =   6240
            TabIndex        =   79
            Top             =   8940
            Width           =   1215
         End
         Begin VB.TextBox txt33b 
            Height          =   285
            Left            =   9840
            TabIndex        =   78
            Top             =   12720
            Width           =   1575
         End
         Begin VB.TextBox txt33a 
            Height          =   285
            Left            =   7920
            TabIndex        =   77
            Top             =   12720
            Width           =   1695
         End
         Begin VB.TextBox txt24Line1Id2 
            Height          =   285
            Left            =   10200
            TabIndex        =   76
            Top             =   9200
            Width           =   1215
         End
         Begin VB.TextBox txt24Line1Qual2 
            Height          =   285
            Left            =   9840
            TabIndex        =   75
            Top             =   9200
            Width           =   375
         End
         Begin VB.TextBox txt24Line1Id1 
            Height          =   285
            Left            =   10200
            TabIndex        =   74
            Top             =   8880
            Width           =   1215
         End
         Begin VB.TextBox txt24Line1Qual1 
            Height          =   285
            Left            =   9840
            TabIndex        =   73
            Top             =   8880
            Width           =   345
         End
         Begin VB.TextBox txtTIN 
            Height          =   285
            Left            =   240
            TabIndex        =   72
            Top             =   11160
            Width           =   1695
         End
         Begin VB.TextBox txtDXCode4 
            Height          =   285
            Left            =   5400
            TabIndex        =   71
            Top             =   7500
            Width           =   975
         End
         Begin VB.TextBox txtDXCode3 
            Height          =   285
            Left            =   3600
            TabIndex        =   70
            Top             =   7500
            Width           =   975
         End
         Begin VB.TextBox txtDXCode2 
            Height          =   285
            Left            =   1920
            TabIndex        =   69
            Top             =   7500
            Width           =   975
         End
         Begin VB.TextBox txtDXCode1 
            Height          =   285
            Left            =   360
            TabIndex        =   68
            Top             =   7500
            Width           =   975
         End
         Begin VB.TextBox txtEIN 
            Height          =   300
            Left            =   2850
            Locked          =   -1  'True
            TabIndex        =   67
            Top             =   11145
            Width           =   315
         End
         Begin VB.TextBox txtSSN 
            Height          =   300
            Left            =   2385
            Locked          =   -1  'True
            TabIndex        =   66
            Top             =   11145
            Width           =   315
         End
         Begin VB.CheckBox chkSexF 
            Caption         =   "F"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   6960
            TabIndex        =   65
            Top             =   690
            Width           =   615
         End
         Begin VB.CheckBox chkSexM 
            Caption         =   "M"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   6360
            TabIndex        =   64
            Top             =   690
            Width           =   615
         End
         Begin VB.CheckBox chkPatientRelationOther 
            Caption         =   "Other"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   6720
            TabIndex        =   63
            Top             =   1200
            Width           =   735
         End
         Begin VB.CheckBox chkPatientRelationChild 
            Caption         =   "Child"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   6000
            TabIndex        =   62
            Top             =   1200
            Width           =   615
         End
         Begin VB.CheckBox chkPatientRelationSpouse 
            Caption         =   "Spouse"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   5160
            TabIndex        =   61
            Top             =   1200
            Width           =   735
         End
         Begin VB.CheckBox chkPatientRelationSelf 
            Caption         =   "Self"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   4560
            TabIndex        =   60
            Top             =   1200
            Width           =   615
         End
         Begin VB.CheckBox chkOthInsN 
            Caption         =   "No"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   8520
            TabIndex        =   59
            Top             =   4560
            Width           =   615
         End
         Begin VB.CheckBox chkOthInsY 
            Caption         =   "Yes"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7800
            TabIndex        =   58
            Top             =   4530
            Width           =   615
         End
         Begin VB.CheckBox chkOthInsdSexF 
            Caption         =   "F"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   3720
            TabIndex        =   57
            Top             =   3540
            Width           =   615
         End
         Begin VB.CheckBox chkOthInsdSexM 
            Caption         =   "M"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   3000
            TabIndex        =   56
            Top             =   3540
            Width           =   615
         End
         Begin VB.CheckBox chkMaritalStatusOther 
            Caption         =   "Other"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   6480
            TabIndex        =   55
            Top             =   1680
            Width           =   975
         End
         Begin VB.CheckBox chkMaritalStatusMarried 
            Caption         =   "Married"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   5520
            TabIndex        =   54
            Top             =   1680
            Width           =   975
         End
         Begin VB.CheckBox chkMaritalStatusSingle 
            Caption         =   "Single"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4560
            TabIndex        =   53
            Top             =   1680
            Width           =   975
         End
         Begin VB.CheckBox chkInsdSexF 
            Caption         =   "F"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   10800
            TabIndex        =   52
            Top             =   3090
            Width           =   615
         End
         Begin VB.CheckBox chkInsdSexM 
            Caption         =   "M"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   10080
            TabIndex        =   51
            Top             =   3090
            Width           =   615
         End
         Begin VB.CheckBox chkEmployStatusPTStudent 
            Caption         =   "PT Student"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   6480
            TabIndex        =   50
            Top             =   2130
            Width           =   975
         End
         Begin VB.CheckBox chkEmployStatusFTStudent 
            Caption         =   "FT Student"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   5520
            TabIndex        =   49
            Top             =   2130
            Width           =   975
         End
         Begin VB.CheckBox chkEmployStatusEmployed 
            Caption         =   "Employed"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4560
            TabIndex        =   48
            Top             =   2130
            Width           =   975
         End
         Begin VB.CheckBox chkConditionOtherY 
            Caption         =   "Yes"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4920
            TabIndex        =   47
            Top             =   4020
            Width           =   615
         End
         Begin VB.CheckBox chkConditionOtherN 
            Caption         =   "No"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   5760
            TabIndex        =   46
            Top             =   4050
            Width           =   615
         End
         Begin VB.CheckBox chkConditionEmployN 
            Caption         =   "No"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   5760
            TabIndex        =   45
            Top             =   2910
            Width           =   615
         End
         Begin VB.CheckBox chkConditionEmployY 
            Caption         =   "Yes"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4920
            TabIndex        =   44
            Top             =   2880
            Width           =   615
         End
         Begin VB.CheckBox chkConditionAutoN 
            Caption         =   "No"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   5760
            TabIndex        =   43
            Top             =   3510
            Width           =   615
         End
         Begin VB.CheckBox chkConditionAutoY 
            Caption         =   "Yes"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   5040
            TabIndex        =   42
            Top             =   3480
            Width           =   615
         End
         Begin VB.TextBox txtBillingState 
            Alignment       =   2  'Center
            Height          =   285
            Left            =   9840
            Locked          =   -1  'True
            MaxLength       =   2
            TabIndex        =   41
            Top             =   12360
            Width           =   375
         End
         Begin VB.TextBox txtBillingZip 
            Height          =   285
            Left            =   10200
            Locked          =   -1  'True
            MaxLength       =   10
            TabIndex        =   40
            Top             =   12360
            Width           =   1215
         End
         Begin VB.TextBox txtBillingCity 
            Height          =   285
            Left            =   7920
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   39
            Top             =   12360
            Width           =   1935
         End
         Begin VB.TextBox txtBillingLine2 
            Height          =   285
            Left            =   7920
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   38
            Top             =   12120
            Width           =   3495
         End
         Begin VB.TextBox txtBillingLine1 
            Height          =   285
            Left            =   7920
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   37
            Top             =   11880
            Width           =   3495
         End
         Begin VB.TextBox txtFacilityZip 
            Height          =   285
            Left            =   6360
            Locked          =   -1  'True
            MaxLength       =   10
            TabIndex        =   36
            Top             =   12360
            Width           =   1215
         End
         Begin VB.TextBox txtFacilityState 
            Alignment       =   2  'Center
            Height          =   285
            Left            =   6000
            Locked          =   -1  'True
            MaxLength       =   2
            TabIndex        =   35
            Top             =   12360
            Width           =   375
         End
         Begin VB.TextBox txtFacilityCity 
            Height          =   285
            Left            =   4080
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   34
            Top             =   12360
            Width           =   1935
         End
         Begin VB.TextBox txtFacilityLine2 
            Height          =   285
            Left            =   4080
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   33
            Top             =   12120
            Width           =   3495
         End
         Begin VB.TextBox txtFacilityLine1 
            Height          =   285
            Left            =   4080
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   32
            Top             =   11880
            Width           =   3495
         End
         Begin VB.CheckBox chkMedicare 
            Caption         =   "(Medicare #)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   0
            MaskColor       =   &H80000000&
            TabIndex        =   31
            Top             =   240
            Width           =   1095
         End
         Begin VB.CheckBox chkMedicaid 
            Caption         =   "(Medicaid #)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   1080
            TabIndex        =   30
            Top             =   240
            Width           =   1095
         End
         Begin VB.CheckBox chkTricare 
            Caption         =   "(ID#/DoD#)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   2160
            TabIndex        =   29
            Top             =   240
            Width           =   1335
         End
         Begin VB.CheckBox chkChampVa 
            Caption         =   "(VA File #)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   3480
            TabIndex        =   28
            Top             =   240
            Width           =   1095
         End
         Begin VB.CheckBox chkGroup 
            Caption         =   "(ID#)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   4560
            TabIndex        =   27
            Top             =   240
            Width           =   1095
         End
         Begin VB.CheckBox chkFeca 
            Caption         =   "(ID#)"
            Enabled         =   0   'False
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   5640
            TabIndex        =   26
            Top             =   240
            Width           =   855
         End
         Begin VB.CheckBox chkOther 
            BackColor       =   &H8000000A&
            Caption         =   "(ID#)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   -1  'True
               Strikethrough   =   0   'False
            EndProperty
            Height          =   195
            Left            =   6600
            MaskColor       =   &H80000009&
            TabIndex        =   25
            Top             =   240
            Value           =   1  'Checked
            Width           =   855
         End
         Begin VB.TextBox txtInsuredID 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   24
            Top             =   180
            Width           =   3615
         End
         Begin VB.TextBox txtPatientLastName 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   23
            Top             =   660
            Width           =   2055
         End
         Begin VB.TextBox txtInsuredLastName 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   22
            Top             =   660
            Width           =   1815
         End
         Begin VB.TextBox txtPatientStreetNum 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   21
            Top             =   1140
            Width           =   4335
         End
         Begin VB.TextBox txtInsuredStreetNum 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   20
            Top             =   1140
            Width           =   3615
         End
         Begin VB.TextBox txtPatientCity 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   19
            Top             =   1620
            Width           =   3735
         End
         Begin VB.TextBox txtPatientState 
            Height          =   285
            Left            =   3960
            Locked          =   -1  'True
            MaxLength       =   2
            TabIndex        =   18
            Top             =   1620
            Width           =   375
         End
         Begin VB.TextBox txtInsuredCity 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   17
            Top             =   1620
            Width           =   2895
         End
         Begin VB.TextBox txtInsuredState 
            Height          =   285
            Left            =   10920
            Locked          =   -1  'True
            MaxLength       =   2
            TabIndex        =   16
            Top             =   1620
            Width           =   375
         End
         Begin VB.TextBox txtPatientZip 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   10
            TabIndex        =   15
            Top             =   2100
            Width           =   1815
         End
         Begin VB.TextBox txtInsuredZip 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   10
            TabIndex        =   14
            Top             =   2100
            Width           =   1455
         End
         Begin VB.TextBox txtOthInsdLastName 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   13
            Top             =   2580
            Width           =   2055
         End
         Begin VB.TextBox txtOthInsdCardNum 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   12
            Top             =   3060
            Width           =   4335
         End
         Begin VB.TextBox txtOthInsdEmployer 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   11
            Top             =   4020
            Width           =   4335
         End
         Begin VB.TextBox txtOthPlanName 
            Height          =   285
            Left            =   0
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   10
            Top             =   4500
            Width           =   3495
         End
         Begin VB.TextBox txtConditionAutoPlace 
            Enabled         =   0   'False
            Height          =   285
            Left            =   6720
            Locked          =   -1  'True
            TabIndex        =   9
            Top             =   3480
            Width           =   375
         End
         Begin VB.TextBox txtSubmissionID 
            Height          =   285
            Left            =   3360
            Locked          =   -1  'True
            TabIndex        =   8
            Top             =   11160
            Width           =   1815
         End
         Begin VB.TextBox txtInsdGroupNum 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   7
            Top             =   2580
            Width           =   3615
         End
         Begin VB.TextBox txtOtherClaimID 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   6
            Top             =   3540
            Width           =   3615
         End
         Begin VB.TextBox txtPlanName 
            Height          =   285
            Left            =   7800
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   5
            Top             =   4020
            Width           =   3615
         End
         Begin VB.TextBox txtPatientSigned 
            Appearance      =   0  'Flat
            BackColor       =   &H8000000A&
            BorderStyle     =   0  'None
            Height          =   285
            Left            =   720
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   4
            TabStop         =   0   'False
            Text            =   "SIGNATURE ON FILE"
            Top             =   5400
            Width           =   3495
         End
         Begin VB.TextBox txtInsuredSigned 
            Appearance      =   0  'Flat
            BackColor       =   &H8000000A&
            BorderStyle     =   0  'None
            Height          =   285
            Left            =   8640
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   3
            TabStop         =   0   'False
            Text            =   "SIGNATURE ON FILE"
            Top             =   5400
            Width           =   2775
         End
         Begin VB.TextBox txtProviderSigned 
            Height          =   285
            Left            =   600
            Locked          =   -1  'True
            MaxLength       =   60
            TabIndex        =   2
            Top             =   12600
            Width           =   1455
         End
         Begin MSMask.MaskEdBox mskLine1DOSTo 
            Height          =   265
            Left            =   1400
            TabIndex        =   88
            Top             =   8940
            Width           =   1095
            _ExtentX        =   1931
            _ExtentY        =   476
            _Version        =   393216
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskLine1DOSFrom 
            Height          =   265
            Left            =   120
            TabIndex        =   89
            Top             =   8940
            Width           =   1095
            _ExtentX        =   1931
            _ExtentY        =   476
            _Version        =   393216
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskCurrentDate 
            Height          =   300
            Left            =   5160
            TabIndex        =   90
            TabStop         =   0   'False
            Top             =   5385
            Width           =   1455
            _ExtentX        =   2566
            _ExtentY        =   529
            _Version        =   393216
            BorderStyle     =   0
            Appearance      =   0
            BackColor       =   -2147483637
            MaxLength       =   10
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskPatientPhone 
            Height          =   285
            Left            =   2400
            TabIndex        =   91
            Top             =   2100
            Width           =   1935
            _ExtentX        =   3413
            _ExtentY        =   503
            _Version        =   393216
            MaxLength       =   14
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Mask            =   "(###)###-####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskPatientDOB 
            Height          =   285
            Left            =   4800
            TabIndex        =   92
            Top             =   660
            Width           =   1215
            _ExtentX        =   2143
            _ExtentY        =   503
            _Version        =   393216
            MaxLength       =   10
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskInsuredPhone 
            Height          =   285
            Left            =   9480
            TabIndex        =   93
            Top             =   2100
            Width           =   1935
            _ExtentX        =   3413
            _ExtentY        =   503
            _Version        =   393216
            MaxLength       =   10
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Mask            =   "(###)###-####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskOthInsdDOB 
            Height          =   285
            Left            =   240
            TabIndex        =   94
            Top             =   3540
            Width           =   1215
            _ExtentX        =   2143
            _ExtentY        =   503
            _Version        =   393216
            MaxLength       =   10
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskInsdDOB 
            Height          =   285
            Left            =   7920
            TabIndex        =   95
            Top             =   3060
            Width           =   1215
            _ExtentX        =   2143
            _ExtentY        =   503
            _Version        =   393216
            MaxLength       =   10
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskDateOfSymptom 
            Height          =   285
            Left            =   240
            TabIndex        =   96
            TabStop         =   0   'False
            Top             =   6000
            Width           =   1215
            _ExtentX        =   2143
            _ExtentY        =   503
            _Version        =   393216
            BorderStyle     =   0
            BackColor       =   -2147483638
            MaxLength       =   10
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin MSMask.MaskEdBox mskProviderSignDate 
            Height          =   285
            Left            =   2040
            TabIndex        =   97
            TabStop         =   0   'False
            Top             =   12600
            Width           =   1215
            _ExtentX        =   2143
            _ExtentY        =   503
            _Version        =   393216
            BorderStyle     =   0
            BackColor       =   -2147483638
            MaxLength       =   10
            Mask            =   "##/##/####"
            PromptChar      =   "_"
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "REFERRING PROVIDER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   53
            Left            =   240
            TabIndex        =   238
            Top             =   6360
            Width           =   3615
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "17."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   52
            Left            =   0
            TabIndex        =   237
            Top             =   6360
            Width           =   255
         End
         Begin VB.Label Label2 
            BackStyle       =   0  'Transparent
            Caption         =   "HOSPITALIZATION DATES"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   8040
            TabIndex        =   236
            Top             =   6480
            Width           =   3375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "18."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   51
            Left            =   7740
            TabIndex        =   235
            Top             =   6360
            Width           =   255
         End
         Begin VB.Label Label1 
            BackStyle       =   0  'Transparent
            Caption         =   "RESUBMISSION CODE - PAYER ICN"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   8040
            TabIndex        =   234
            Top             =   7320
            Width           =   3375
         End
         Begin VB.Label lbl 
            Caption         =   ","
            Height          =   255
            Index           =   50
            Left            =   2040
            TabIndex        =   233
            Top             =   2640
            Width           =   135
         End
         Begin VB.Label lbl 
            Caption         =   ","
            Height          =   255
            Index           =   49
            Left            =   9600
            TabIndex        =   232
            Top             =   735
            Width           =   135
         End
         Begin VB.Label lbl 
            Caption         =   ","
            Height          =   255
            Index           =   48
            Left            =   2040
            TabIndex        =   231
            Top             =   740
            Width           =   135
         End
         Begin VB.Label Label65 
            BackStyle       =   0  'Transparent
            Caption         =   "22."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   230
            Top             =   7320
            Width           =   255
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "J."
            Height          =   255
            Index           =   10
            Left            =   10680
            TabIndex        =   229
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "H."
            Height          =   255
            Index           =   8
            Left            =   9550
            TabIndex        =   228
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "I."
            Height          =   255
            Index           =   9
            Left            =   9960
            TabIndex        =   227
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lblLine19 
            BackStyle       =   0  'Transparent
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   495
            Left            =   2160
            TabIndex        =   226
            Top             =   6900
            Width           =   3735
         End
         Begin VB.Label lblPatID 
            BackStyle       =   0  'Transparent
            Caption         =   "26. PATIENT'S ACCOUNT NUMBER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   390
            Left            =   3330
            TabIndex        =   225
            Top             =   10950
            Width           =   2550
         End
         Begin VB.Line Line32 
            X1              =   3285
            X2              =   3285
            Y1              =   10920
            Y2              =   12975
         End
         Begin VB.Label Label98 
            Caption         =   "EIN"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   225
            Left            =   2880
            TabIndex        =   224
            Top             =   10950
            Width           =   345
         End
         Begin VB.Label Label97 
            Caption         =   "SSN"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   180
            Left            =   2415
            TabIndex        =   223
            Top             =   10950
            Width           =   360
         End
         Begin VB.Label Label93 
            Caption         =   "25. FEDERAL TAX ID NUMBER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   222
            Top             =   10950
            Width           =   2175
         End
         Begin VB.Label lblFreeTextCertNum 
            BackStyle       =   0  'Transparent
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   8040
            TabIndex        =   221
            Top             =   7980
            Width           =   2895
         End
         Begin VB.Label lblDX1Units 
            Alignment       =   2  'Center
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   9060
            TabIndex        =   220
            Top             =   8940
            Width           =   375
         End
         Begin VB.Label lblDX1Charges 
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   7800
            TabIndex        =   219
            Top             =   8940
            Width           =   1095
         End
         Begin VB.Label lblDX1CPTCode 
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   3600
            TabIndex        =   218
            Top             =   8940
            Width           =   975
         End
         Begin VB.Label lblTOS 
            Alignment       =   2  'Center
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   3060
            TabIndex        =   217
            Top             =   8940
            Width           =   375
         End
         Begin VB.Label lblDX1POS 
            Alignment       =   2  'Center
            BorderStyle     =   1  'Fixed Single
            Height          =   285
            Left            =   2580
            TabIndex        =   216
            Top             =   8940
            Width           =   375
         End
         Begin VB.Line Line1 
            BorderWidth     =   3
            X1              =   0
            X2              =   11400
            Y1              =   5760
            Y2              =   5760
         End
         Begin VB.Line Line2 
            BorderWidth     =   2
            X1              =   7680
            X2              =   7680
            Y1              =   0
            Y2              =   12960
         End
         Begin VB.Line Line3 
            Index           =   0
            X1              =   0
            X2              =   11400
            Y1              =   480
            Y2              =   480
         End
         Begin VB.Line Line4 
            X1              =   0
            X2              =   11400
            Y1              =   960
            Y2              =   960
         End
         Begin VB.Line Line5 
            X1              =   0
            X2              =   11400
            Y1              =   1440
            Y2              =   1440
         End
         Begin VB.Line Line6 
            X1              =   0
            X2              =   4440
            Y1              =   2880
            Y2              =   2880
         End
         Begin VB.Line Line7 
            X1              =   0
            X2              =   4440
            Y1              =   1920
            Y2              =   1920
         End
         Begin VB.Line Line8 
            X1              =   7680
            X2              =   11400
            Y1              =   1920
            Y2              =   1920
         End
         Begin VB.Line Line3 
            Index           =   1
            X1              =   0
            X2              =   11400
            Y1              =   2400
            Y2              =   2400
         End
         Begin VB.Line Line3 
            Index           =   2
            X1              =   0
            X2              =   4440
            Y1              =   3360
            Y2              =   3360
         End
         Begin VB.Line Line3 
            Index           =   3
            X1              =   0
            X2              =   4440
            Y1              =   3840
            Y2              =   3840
         End
         Begin VB.Line Line3 
            Index           =   4
            X1              =   0
            X2              =   11400
            Y1              =   4320
            Y2              =   4320
         End
         Begin VB.Line Line3 
            Index           =   5
            X1              =   0
            X2              =   11400
            Y1              =   4800
            Y2              =   4800
         End
         Begin VB.Line Line3 
            Index           =   6
            X1              =   0
            X2              =   11400
            Y1              =   6360
            Y2              =   6360
         End
         Begin VB.Line Line3 
            Index           =   7
            X1              =   7680
            X2              =   11400
            Y1              =   2880
            Y2              =   2880
         End
         Begin VB.Line Line3 
            Index           =   8
            X1              =   7680
            X2              =   11400
            Y1              =   3360
            Y2              =   3360
         End
         Begin VB.Line Line3 
            Index           =   9
            X1              =   7680
            X2              =   11400
            Y1              =   3840
            Y2              =   3840
         End
         Begin VB.Line Line3 
            Index           =   10
            X1              =   0
            X2              =   11400
            Y1              =   6840
            Y2              =   6840
         End
         Begin VB.Line Line3 
            Index           =   11
            X1              =   0
            X2              =   11400
            Y1              =   7320
            Y2              =   7320
         End
         Begin VB.Label lbl 
            Caption         =   " MEDICARE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   215
            Top             =   0
            Width           =   975
         End
         Begin VB.Label lbl 
            Caption         =   "MEDICAID"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   1
            Left            =   3360
            TabIndex        =   214
            Top             =   600
            Width           =   855
         End
         Begin VB.Label lbl 
            Caption         =   "TRICARE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   2
            Left            =   2400
            TabIndex        =   213
            Top             =   0
            Width           =   735
         End
         Begin VB.Label lbl 
            Caption         =   "CHAMPVA"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   3
            Left            =   3720
            TabIndex        =   212
            Top             =   0
            Width           =   855
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "GROUP"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   4
            Left            =   4800
            TabIndex        =   211
            Top             =   0
            Width           =   1095
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   " FECA "
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   5
            Left            =   5880
            TabIndex        =   210
            Top             =   0
            Width           =   1095
         End
         Begin VB.Label lbl 
            Caption         =   "OTHER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   6
            Left            =   6840
            TabIndex        =   209
            Top             =   0
            Width           =   375
         End
         Begin VB.Label Label8 
            Caption         =   "1."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   208
            Top             =   0
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "1a."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   7
            Left            =   7740
            TabIndex        =   207
            Top             =   0
            Width           =   255
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S I.D. NUMBER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   8
            Left            =   7980
            TabIndex        =   206
            Top             =   0
            Width           =   1455
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "2."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   9
            Left            =   0
            TabIndex        =   205
            Top             =   480
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S NAME (Last Name , First Name, Middle Initial)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   10
            Left            =   240
            TabIndex        =   204
            Top             =   480
            Width           =   3375
         End
         Begin VB.Line Line9 
            X1              =   4440
            X2              =   4440
            Y1              =   480
            Y2              =   4800
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "3."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   11
            Left            =   4500
            TabIndex        =   203
            Top             =   480
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S BIRTHDATE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   12
            Left            =   4740
            TabIndex        =   202
            Top             =   480
            Width           =   1335
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "SEX"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   13
            Left            =   6720
            TabIndex        =   201
            Top             =   480
            Width           =   375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "4."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   14
            Left            =   7740
            TabIndex        =   200
            Top             =   480
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S NAME (Last Name , First Name, Middle Initial)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   15
            Left            =   7935
            TabIndex        =   199
            Top             =   480
            Width           =   3375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "5."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   16
            Left            =   0
            TabIndex        =   198
            Top             =   960
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S ADDRESS (No. Street)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   17
            Left            =   240
            TabIndex        =   197
            Top             =   960
            Width           =   3375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "6."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   18
            Left            =   4500
            TabIndex        =   196
            Top             =   960
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S RELATIONSHIP TO INSURED"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   19
            Left            =   4680
            TabIndex        =   195
            Top             =   960
            Width           =   2775
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "7."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   20
            Left            =   7740
            TabIndex        =   194
            Top             =   960
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S ADDRESS (No. Street)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   21
            Left            =   7920
            TabIndex        =   193
            Top             =   960
            Width           =   3375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "CITY"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   22
            Left            =   240
            TabIndex        =   192
            Top             =   1440
            Width           =   495
         End
         Begin VB.Line Line10 
            X1              =   3840
            X2              =   3840
            Y1              =   1440
            Y2              =   1920
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "STATE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   23
            Left            =   3960
            TabIndex        =   191
            Top             =   1440
            Width           =   375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "8."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   24
            Left            =   4500
            TabIndex        =   190
            Top             =   1440
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S STATUS"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   25
            Left            =   4680
            TabIndex        =   189
            Top             =   1440
            Width           =   1335
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "CITY"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   26
            Left            =   7920
            TabIndex        =   188
            Top             =   1440
            Width           =   495
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "ZIP CODE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   28
            Left            =   240
            TabIndex        =   187
            Top             =   1920
            Width           =   615
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "ZIP CODE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   30
            Left            =   8160
            TabIndex        =   186
            Top             =   1920
            Width           =   615
         End
         Begin VB.Line Line11 
            X1              =   10800
            X2              =   10800
            Y1              =   1440
            Y2              =   1920
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "STATE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   27
            Left            =   10920
            TabIndex        =   185
            Top             =   1440
            Width           =   375
         End
         Begin VB.Line Line12 
            X1              =   2280
            X2              =   2280
            Y1              =   1920
            Y2              =   2400
         End
         Begin VB.Line Line13 
            X1              =   9360
            X2              =   9360
            Y1              =   1920
            Y2              =   2400
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "TELEPHONE (Include Area Code)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   29
            Left            =   2400
            TabIndex        =   184
            Top             =   1920
            Width           =   1815
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "TELEPHONE (Include Area Code)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   31
            Left            =   9480
            TabIndex        =   183
            Top             =   1920
            Width           =   1815
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "9."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   32
            Left            =   0
            TabIndex        =   182
            Top             =   2400
            Width           =   135
         End
         Begin VB.Label lblOthInsdLastName 
            BackStyle       =   0  'Transparent
            Caption         =   "SECONDARY INSURED NAME (Last Name , First Name, M.I.)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   181
            Top             =   2400
            Width           =   3495
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "10."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   34
            Left            =   4470
            TabIndex        =   180
            Top             =   2400
            Width           =   255
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "IS PATIENT'S CONDITION RELATED TO:"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   35
            Left            =   4680
            TabIndex        =   179
            Top             =   2400
            Width           =   3015
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "a."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   38
            Left            =   0
            TabIndex        =   178
            Top             =   2880
            Width           =   135
         End
         Begin VB.Label lblOthInsdCardNum 
            BackStyle       =   0  'Transparent
            Caption         =   "SECONDARY INSURED POLICY ID"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   177
            Top             =   2880
            Width           =   3495
         End
         Begin VB.Label Label41 
            BackStyle       =   0  'Transparent
            Caption         =   "b."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   176
            Top             =   3360
            Width           =   135
         End
         Begin VB.Label lblOthInsdDOB 
            BackStyle       =   0  'Transparent
            Caption         =   "SECONDARY INSURED DATE OF BIRTH"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   175
            Top             =   3360
            Width           =   3015
         End
         Begin VB.Label lblOthInsdSexF 
            BackStyle       =   0  'Transparent
            Caption         =   "SEX"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   3360
            TabIndex        =   174
            Top             =   3360
            Width           =   375
         End
         Begin VB.Label Label44 
            BackStyle       =   0  'Transparent
            Caption         =   "c."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   173
            Top             =   3840
            Width           =   135
         End
         Begin VB.Label lblOthInsdEmployer 
            BackStyle       =   0  'Transparent
            Caption         =   "SECONDARY EMPLOYER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   172
            Top             =   3840
            Width           =   3495
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "d."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   58
            Left            =   0
            TabIndex        =   171
            Top             =   4320
            Width           =   135
         End
         Begin VB.Label lblOthPayerName 
            BackStyle       =   0  'Transparent
            Caption         =   "SECONDARY PAYER NAME"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   170
            Top             =   4320
            Width           =   3495
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "a. EMPLOYMENT? (CURRENT OR PREVIOUS)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   40
            Left            =   4560
            TabIndex        =   169
            Top             =   2640
            Width           =   3135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "b. AUTO ACCIDENT?"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   41
            Left            =   4560
            TabIndex        =   168
            Top             =   3240
            Width           =   1455
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "PLACE (State)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   42
            Left            =   6480
            TabIndex        =   167
            Top             =   3240
            Width           =   975
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "c. OTHER ACCIDENT?"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   43
            Left            =   4560
            TabIndex        =   166
            Top             =   3840
            Width           =   1455
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "10d."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   55
            Left            =   4470
            TabIndex        =   165
            Top             =   4320
            Width           =   255
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "CLAIM CODES"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   54
            Left            =   4800
            TabIndex        =   164
            Top             =   4320
            Width           =   2055
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "11."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   36
            Left            =   7740
            TabIndex        =   163
            Top             =   2400
            Width           =   255
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S POLICY GROUP"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   37
            Left            =   7920
            TabIndex        =   162
            Top             =   2400
            Width           =   3375
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "a."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   44
            Left            =   7740
            TabIndex        =   161
            Top             =   2880
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S DATE OF BIRTH"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   45
            Left            =   7920
            TabIndex        =   160
            Top             =   2880
            Width           =   1695
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "SEX"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   46
            Left            =   10440
            TabIndex        =   159
            Top             =   2880
            Width           =   375
         End
         Begin VB.Label Label59 
            BackStyle       =   0  'Transparent
            Caption         =   "b."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   158
            Top             =   3360
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "OTHER CLAIM ID (Designated by NUCC)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   47
            Left            =   7920
            TabIndex        =   157
            Top             =   3360
            Width           =   3255
         End
         Begin VB.Label Label61 
            BackStyle       =   0  'Transparent
            Caption         =   "c."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   156
            Top             =   3840
            Width           =   135
         End
         Begin VB.Label Label62 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURANCE PLAN NAME OR PROGRAM NAME"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   7920
            TabIndex        =   155
            Top             =   3840
            Width           =   3495
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "d."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   56
            Left            =   7740
            TabIndex        =   154
            Top             =   4320
            Width           =   135
         End
         Begin VB.Label lbl 
            BackStyle       =   0  'Transparent
            Caption         =   "IS THERE ANOTHER HEALTH BENEFIT PLAN?"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Index           =   57
            Left            =   7920
            TabIndex        =   153
            Top             =   4320
            Width           =   3375
         End
         Begin VB.Label Label66 
            BackStyle       =   0  'Transparent
            Caption         =   "READ BACK OF FORM BEFORE COMPLETING AND SIGNING THIS FORM"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   1080
            TabIndex        =   152
            Top             =   4800
            Width           =   5895
         End
         Begin VB.Label Label67 
            BackStyle       =   0  'Transparent
            Caption         =   "12."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   151
            Top             =   4920
            Width           =   255
         End
         Begin VB.Label Label68 
            BackStyle       =   0  'Transparent
            Caption         =   "PATIENT'S OR AUTHORIZED PERSON'S SIGNATURE."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   150
            Top             =   4920
            Width           =   3615
         End
         Begin VB.Label Label69 
            BackStyle       =   0  'Transparent
            Caption         =   "I authorize the release of any medical or other information"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   3720
            TabIndex        =   149
            Top             =   4920
            Width           =   3615
         End
         Begin VB.Label Label70 
            BackStyle       =   0  'Transparent
            Caption         =   "necessary to process this claim. I also request payment of government benifits either to myself or to the party who"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   240
            TabIndex        =   148
            Top             =   5040
            Width           =   7575
         End
         Begin VB.Label Label71 
            BackStyle       =   0  'Transparent
            Caption         =   "accepts assingment below."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   240
            TabIndex        =   147
            Top             =   5160
            Width           =   4215
         End
         Begin VB.Label Label72 
            BackStyle       =   0  'Transparent
            Caption         =   "SIGNED"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   120
            TabIndex        =   146
            Top             =   5520
            Width           =   735
         End
         Begin VB.Label Label73 
            BackStyle       =   0  'Transparent
            Caption         =   "DATE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4560
            TabIndex        =   145
            Top             =   5520
            Width           =   615
         End
         Begin VB.Label Label74 
            BackStyle       =   0  'Transparent
            Caption         =   "13."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   144
            Top             =   4800
            Width           =   255
         End
         Begin VB.Label Label75 
            BackStyle       =   0  'Transparent
            Caption         =   "INSURED'S OR AUTHORIZED PERSON'S SIGNATURE."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   7920
            TabIndex        =   143
            Top             =   4800
            Width           =   3615
         End
         Begin VB.Label Label77 
            BackStyle       =   0  'Transparent
            Caption         =   "SIGNED"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7920
            TabIndex        =   142
            Top             =   5520
            Width           =   735
         End
         Begin VB.Label Label78 
            BackStyle       =   0  'Transparent
            Caption         =   "14."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   141
            Top             =   5820
            Width           =   255
         End
         Begin VB.Label Label79 
            BackStyle       =   0  'Transparent
            Caption         =   "DATE OF CURRENT:"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   140
            Top             =   5820
            Width           =   1215
         End
         Begin VB.Image Image1 
            Height          =   480
            Left            =   1560
            Top             =   5880
            Width           =   480
         End
         Begin VB.Label Label80 
            BackStyle       =   0  'Transparent
            Caption         =   "ILLNESS (First syptom) OR INJURY (Accident) OR PREGNANCY (LMP)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   495
            Left            =   2160
            TabIndex        =   139
            Top             =   5820
            Width           =   1695
         End
         Begin VB.Line Line14 
            X1              =   3960
            X2              =   3960
            Y1              =   5760
            Y2              =   6840
         End
         Begin VB.Label Label86 
            BackStyle       =   0  'Transparent
            Caption         =   "HEALTH PLAN"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4800
            TabIndex        =   138
            Top             =   120
            Width           =   1095
         End
         Begin VB.Label Label87 
            BackStyle       =   0  'Transparent
            Caption         =   "BLK LUNG"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   5880
            TabIndex        =   137
            Top             =   120
            Width           =   1095
         End
         Begin VB.Label Label94 
            BackStyle       =   0  'Transparent
            Caption         =   "19."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   136
            Top             =   6840
            Width           =   255
         End
         Begin VB.Label Label95 
            BackStyle       =   0  'Transparent
            Caption         =   "RESERVED FOR LOCAL USE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   135
            Top             =   6840
            Width           =   3255
         End
         Begin VB.Line Line15 
            X1              =   0
            X2              =   11400
            Y1              =   8280
            Y2              =   8280
         End
         Begin VB.Label Label99 
            BackStyle       =   0  'Transparent
            Caption         =   "21."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   134
            Top             =   7320
            Width           =   255
         End
         Begin VB.Label Label100 
            BackStyle       =   0  'Transparent
            Caption         =   "DIAGNOSIS OR NATURE OF ILLNESS OR INJURY. (RELATE ITEMS 1, 2, 3, OR 4 TO ITEM 24E BY LINE)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   240
            TabIndex        =   133
            Top             =   7320
            Width           =   6495
         End
         Begin VB.Line Line16 
            X1              =   7680
            X2              =   11400
            Y1              =   7800
            Y2              =   7800
         End
         Begin VB.Label lblDXCode 
            Caption         =   "A."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   0
            Left            =   120
            TabIndex        =   132
            Top             =   7560
            Width           =   135
         End
         Begin VB.Label lblDXCode 
            BackStyle       =   0  'Transparent
            Caption         =   "B."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   1
            Left            =   1680
            TabIndex        =   131
            Top             =   7560
            Width           =   135
         End
         Begin VB.Label lblDXCode 
            BackStyle       =   0  'Transparent
            Caption         =   "C."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   2
            Left            =   3360
            TabIndex        =   130
            Top             =   7560
            Width           =   135
         End
         Begin VB.Label lblDXCode 
            BackStyle       =   0  'Transparent
            Caption         =   "D."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   3
            Left            =   5160
            TabIndex        =   129
            Top             =   7560
            Width           =   135
         End
         Begin VB.Label Label106 
            BackStyle       =   0  'Transparent
            Caption         =   "23."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   128
            Top             =   7800
            Width           =   255
         End
         Begin VB.Label Label108 
            BackStyle       =   0  'Transparent
            Caption         =   "PRIOR AUTHORIZATION NUMBER"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   8040
            TabIndex        =   127
            Top             =   7800
            Width           =   2175
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "24."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   0
            Left            =   0
            TabIndex        =   126
            Top             =   8280
            Width           =   255
         End
         Begin VB.Line Line18 
            X1              =   6120
            X2              =   6120
            Y1              =   8280
            Y2              =   10920
         End
         Begin VB.Line Line19 
            X1              =   3480
            X2              =   3480
            Y1              =   8280
            Y2              =   10920
         End
         Begin VB.Line Line20 
            X1              =   3000
            X2              =   3000
            Y1              =   8280
            Y2              =   10920
         End
         Begin VB.Line Line21 
            X1              =   2520
            X2              =   2520
            Y1              =   8280
            Y2              =   10920
         End
         Begin VB.Line Line22 
            X1              =   9000
            X2              =   9000
            Y1              =   8280
            Y2              =   11520
         End
         Begin VB.Line Line23 
            X1              =   9480
            X2              =   9480
            Y1              =   8280
            Y2              =   11520
         End
         Begin VB.Line Line24 
            X1              =   9840
            X2              =   9840
            Y1              =   8280
            Y2              =   11520
         End
         Begin VB.Line Line25 
            X1              =   10200
            X2              =   10200
            Y1              =   8280
            Y2              =   11520
         End
         Begin VB.Line Line27 
            X1              =   0
            X2              =   11400
            Y1              =   8880
            Y2              =   8880
         End
         Begin VB.Line Line28 
            X1              =   0
            X2              =   11400
            Y1              =   9480
            Y2              =   9480
         End
         Begin VB.Line Line29 
            X1              =   0
            X2              =   11400
            Y1              =   9960
            Y2              =   9960
         End
         Begin VB.Line Line30 
            X1              =   0
            X2              =   11400
            Y1              =   10440
            Y2              =   10440
         End
         Begin VB.Line Line31 
            X1              =   0
            X2              =   11400
            Y1              =   10920
            Y2              =   10920
         End
         Begin VB.Line Line33 
            X1              =   0
            X2              =   11400
            Y1              =   11520
            Y2              =   11520
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "A."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   1
            Left            =   480
            TabIndex        =   125
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "B."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   2
            Left            =   2640
            TabIndex        =   124
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "C."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   3
            Left            =   3120
            TabIndex        =   123
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "D."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   4
            Left            =   4680
            TabIndex        =   122
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "E."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   5
            Left            =   6840
            TabIndex        =   121
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            BackStyle       =   0  'Transparent
            Caption         =   "F."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   6
            Left            =   8280
            TabIndex        =   120
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label lbl24 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "G."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   7
            Left            =   9120
            TabIndex        =   119
            Top             =   8280
            Width           =   255
         End
         Begin VB.Label Label121 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "DATE(S) OF SERVICE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   0
            TabIndex        =   118
            Top             =   8520
            Width           =   2535
         End
         Begin VB.Label Label122 
            BackStyle       =   0  'Transparent
            Caption         =   "             From                                         To  "
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   0
            TabIndex        =   117
            Top             =   8700
            Width           =   2535
         End
         Begin VB.Label Label123 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "Place  of Service"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   375
            Left            =   2520
            TabIndex        =   116
            Top             =   8520
            Width           =   495
         End
         Begin VB.Label Label124 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "Type  of Service"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   375
            Left            =   3000
            TabIndex        =   115
            Top             =   8520
            Width           =   495
         End
         Begin VB.Label Label125 
            BackStyle       =   0  'Transparent
            Caption         =   "PROCEDURES, SERVICES, OR SUPPLIES"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   3600
            TabIndex        =   114
            Top             =   8460
            Width           =   2535
         End
         Begin VB.Label Label126 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "(Explain Unusual Circumstances)"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   3480
            TabIndex        =   113
            Top             =   8580
            Width           =   2655
         End
         Begin VB.Label Label127 
            BackStyle       =   0  'Transparent
            Caption         =   "  CPT/HCPCS"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   3600
            TabIndex        =   112
            Top             =   8760
            Width           =   2415
         End
         Begin VB.Label Label128 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "DIAGNOSIS CODE"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   6120
            TabIndex        =   111
            Top             =   8520
            Width           =   1575
         End
         Begin VB.Label Label129 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "$ CHARGES"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   135
            Left            =   7680
            TabIndex        =   110
            Top             =   8520
            Width           =   1335
         End
         Begin VB.Label Label130 
            Alignment       =   2  'Center
            BackStyle       =   0  'Transparent
            Caption         =   "DAYS OR UNITS"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   495
            Left            =   9000
            TabIndex        =   109
            Top             =   8460
            Width           =   495
         End
         Begin VB.Label Label135 
            Caption         =   "1."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   108
            Top             =   8940
            Width           =   135
         End
         Begin VB.Label Label81 
            BackStyle       =   0  'Transparent
            Caption         =   "2."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   107
            Top             =   9480
            Width           =   135
         End
         Begin VB.Label Label82 
            BackStyle       =   0  'Transparent
            Caption         =   "3."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   106
            Top             =   9960
            Width           =   135
         End
         Begin VB.Label Label83 
            BackStyle       =   0  'Transparent
            Caption         =   "4."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   105
            Top             =   10440
            Width           =   135
         End
         Begin VB.Label Label84 
            BackStyle       =   0  'Transparent
            Caption         =   "SIGNED"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   104
            Top             =   12720
            Width           =   735
         End
         Begin VB.Label Label85 
            BackStyle       =   0  'Transparent
            Caption         =   "31."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   0
            TabIndex        =   103
            Top             =   11520
            Width           =   255
         End
         Begin VB.Label Label88 
            BackStyle       =   0  'Transparent
            Caption         =   "32."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   4080
            TabIndex        =   102
            Top             =   11520
            Width           =   255
         End
         Begin VB.Label Label89 
            BackStyle       =   0  'Transparent
            Caption         =   "33."
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Left            =   7740
            TabIndex        =   101
            Top             =   11520
            Width           =   255
         End
         Begin VB.Label Label90 
            BackStyle       =   0  'Transparent
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   735
            Left            =   240
            TabIndex        =   100
            Top             =   11040
            Width           =   2895
         End
         Begin VB.Label Label91 
            BackStyle       =   0  'Transparent
            Caption         =   "SERVICE FACILITY LOCATION INFORMATION"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   495
            Left            =   4440
            TabIndex        =   99
            Top             =   11520
            Width           =   3135
         End
         Begin VB.Label Label92 
            BackStyle       =   0  'Transparent
            Caption         =   "BILLING PROVIDER INFO"
            BeginProperty Font 
               Name            =   "MS Serif"
               Size            =   6.75
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   375
            Left            =   8040
            TabIndex        =   98
            Top             =   11520
            Width           =   3375
         End
         Begin VB.Line Line17 
            X1              =   0
            X2              =   11400
            Y1              =   8460
            Y2              =   8460
         End
      End
   End
   Begin VB.Image Image2 
      Height          =   12015
      Left            =   480
      Top             =   240
      Width           =   6015
   End
End
Attribute VB_Name = "frmCMS1500"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private v_ConnectionString As String
Private mlog As Log

Private Sub Form_Load()
   v_ConnectionString = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"
   Dim r As Integer
   r = 0
    Set mlog = New Log
    mlog.ConnectionString = v_ConnectionString
  r = mlog.LogAction(23, 0, 0, 0, 0, "amp CMS 1500 start", "CMS1500 status")
   
   
    
    
  
  
  Clean
  PopulateForm
  
  '  Dim objClaim As ClaimBz.CClaimBz
 '   Dim rst As ADODB.Recordset
 '   Dim strMarStatus As String
 '   Dim strEmpStatus As String
 '   Dim strRelation As String

   
 '   Set objClaim = CreateObject("ClaimBz.CClaimBz")
 '   Set rst = objClaim.FetchByID(lngClaimID)
End Sub



Private Sub Clean()
txtProviderSigned.Text = Empty

txtInsuredSigned.Text = Empty
txtPatientSigned.Text = Empty
txtPlanName.Text = Empty
txtOtherClaimID.Text = Empty
txtInsdGroupNum.Text = Empty
txtSubmissionID.Text = Empty
txtConditionAutoPlace.Text = Empty
txtOthPlanName.Text = Empty
txtOthInsdEmployer.Text = Empty
txtOthInsdCardNum.Text = Empty
txtOthInsdLastName.Text = Empty
txtInsuredZip.Text = Empty
txtPatientZip.Text = Empty
txtInsuredState.Text = Empty
txtInsuredCity.Text = Empty
txtPatientState.Text = Empty
txtPatientCity.Text = Empty
txtInsuredStreetNum.Text = Empty
txtPatientStreetNum.Text = Empty
txtInsuredLastName.Text = Empty
txtPatientLastName.Text = Empty
txtInsuredID.Text = Empty
txtFacilityLine1.Text = Empty
txtFacilityLine2.Text = Empty
txtFacilityCity.Text = Empty
txtFacilityState.Text = Empty
txtFacilityZip.Text = Empty
txtBillingLine1.Text = Empty
txtBillingLine2.Text = Empty
txtBillingCity.Text = Empty
txtBillingZip.Text = Empty
txtBillingState.Text = Empty
txtSSN.Text = Empty
txtEIN.Text = Empty
txtDXCode1.Text = Empty
txtDXCode2.Text = Empty
txtDXCode3.Text = Empty
txtDXCode4.Text = Empty
txtTIN.Text = Empty
txt24Line1Qual1.Text = Empty
txt24Line1Id1.Text = Empty
txt24Line1Qual2.Text = Empty
txt24Line1Id2.Text = Empty
txt33a.Text = Empty
txt33b.Text = Empty
txtLine1DX.Text = Empty
txt32b.Text = Empty
txt32a.Text = Empty
txtPatientFirstName.Text = Empty
txtPatientMI.Text = Empty
txtInsuredFirstName.Text = Empty
txtInsuredMI.Text = Empty
txtOthInsdFirstName.Text = Empty
txtOthInsdMI.Text = Empty


End Sub


Private Sub Controlss()
Dim ControlList As String

Dim ctrl As Control
        For Each ctrl In Me.Controls
        
        
        If TypeOf ctrl Is TextBox Then
        Debug.Print ctrl.Name + vbLf + vbCr
        
        ControlList = ControlList + ctrl.Name & vbLf + vbCr
         
        
        
        End If
        
        
        
        
        
        
        Next
        
        
        Text1.Text = ControlList

End Sub

Private Sub PopulateForm()

Dim pc As psyClaims
Set pc = New psyClaims





End Sub

