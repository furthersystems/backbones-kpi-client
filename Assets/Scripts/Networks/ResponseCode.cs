//------------------------------------------------------------------------------
// <copyright file="ResponseCode.cs" company="FurtherSystem Co.,Ltd.">
// Copyright (C) 2020 FurtherSystem Co.,Ltd.
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
// </copyright>
// <author>FurtherSystem Co.,Ltd.</author>
// <email>info@furthersystem.com</email>
// <summary>
// vQL response code
// </summary>
//------------------------------------------------------------------------------
using System;

namespace Com.FurtherSystems.vQL.Client
{
    public enum ResponseCode : Int16
    {
        // Common XX0XX
        ResponseOk = 0,  // ok, success
        ResponseNgDefault = 1,  // ng, failed provisional error. should not use this code.
        ResponseOkContinue = -1, // ok, polling continue.
        ResponseNgSecSquashed = 2,  // ng, cannot open details for security.
        ResponseNgServerTimeout = 10, // ng, server timeout.
        ResponseNgClientTimeout = 11, // ng, client timeout.
        ResponseNgEncodeInvalid = 12, // ng, encode failed.
        ResponseNgHashGenerateFailed = 13, // ng, hash generate failed.
        ResponseNgShardConnectFailed = 14, // ng, db shard connect failed.
        ResponseNgTransactBeginFailed = 15, // ng, db transaction failed.
        ResponseNgPreparedStatementFailed = 16, // ng, db prepared statement create failed.
        ResponseNgQueryExecuteFailed = 17, // ng, db query execute failed.
        ResponseNgRollbackFailed = 18, // ng, db rollback failed.
        ResponseNgCommitFailed = 19, // ng, db commit failed.
        // VendorRegist XX1XX
        ResponseNgVendorNameBlank = 100, // ng, vendor name is blank.
        ResponseNgVendorNameMaxover = 101, // ng, vendor name is capacity over.
        ResponseNgVendorNameInvalid = 102, // ng, vendor name is invalid. bad caractor.
        ResponseNgVendorCaptionMaxover = 103, // ng, vendor caption is capacity over.
        ResponseNgVendorCaptionInvalid = 104, // ng, vendor caption is invalid. bad caractor.
        ResponseNgNonceInvalid = 105, // ng, nonce invalid.
        ResponseNgTicksInvalid = 106, // ng, ticks invalid.
        ResponseNgSeedInvalid = 107,  // ng, seed verify failed.
        // VendorRegist RecoverCode XX1XX
        ResponseOkVendorAccountBadPrivkeyExistsSeed = -100, // ok, bad private key but, exists seed.
        ResponseOkVendorAccountBadPrivkeyExistsSso = -101, // ok, bad private key but, exists sso.
        ResponseOkVendorAccountNoPrivkeyExistsSeed = -102, // ok, no private key but, exists seed.
        ResponseOkVendorAccountNoPrivkeyExistsSso = -103, // ok, no private key but, exists sso.
        ResponseOkVendorAccountRecoveredDataInvalid = -110, // ok, vendor account invalid data recovered.
        ResponseOkVendorAccountRecoveredFromSeed = -111, // ok, vendor account recovered from seed.
        ResponseOkVendorAccountRecoveredFromSso = -112, // ok, vendor account recovered from sso.
        // VendorView XX2XX
        ResponseNgVendorConnotMoveup = 200, // ng, this is top, cannot more moveup
        ResponseNgVendorAlreadyShelved = 201, // ng, already sheleved.
        ResponseNgVendorAlreadyUnshelved = 202, // ng, already unshelved.
        ResponseNgVendorAlreadyCanceled = 203, // ng, already canceled by vendor.
        // VendorDequeueAuth XX3XX
        ResponseNgVendorCannotAuthDequeue = 300, // ng, user dequeue auth not executed, dequeue auth failed.
        // VendorNotify XX4XX
        // VendorAuthOption XX5XX
        ResponseNgVendorAuthLacked = 500, // ng, vendor auth info lacked.
        ResponseNgVendorAuthFailed = 501, // ng, vendor auth failed.
        // UserQueing XX6XX
        ResponseNgUserMaxover = 600, // ng, user cannot queing, user max over.
        ResponseNgUserOutoftime = 601, // ng, user cannot queing, out of time.
        // UserView XX7XX
        ResponseNgUserAlreadyMailOn = 700, // ng, user already mail on.
        ResponseNgUserAlreadyMailOff = 701, // ng, user already mail off.
        ResponseNgUserAlreadyPushOn = 702, // ng, user already push on.
        ResponseNgUserAlreadyPushOff = 703, // ng, user already push off.
        ResponseNgUserCannotPending = 704, // ng, this is end. cannot more pending.
        ResponseNgUserAlreadyCanceled = 705, // ng, already canceled by user.
        // UserDequeueAuth XX8XX
        ResponseNgUserCannotAuthDequeue = 800, // ng, vendor dequeue auth not executed on time, dequeue auth failed.
        // UserAuthOption XX9XX
        ResponseNgUserAuthLacked = 900, // ng, user auth info lacked.
        ResponseNgUserAuthFailed = 901, // ng, user auth failed.
        ResponseNgUserAuthNotFound = 902, // ng, user auth not found.
    }
}