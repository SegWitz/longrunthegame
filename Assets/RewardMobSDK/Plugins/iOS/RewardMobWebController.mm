//
//  RewardMobWebController.m
//  UnityNativePlugin
//
//  Created by Alex Saunders on 2017-09-22.
//  Copyright © 2017 Alex Saunders. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RewardMobWebController.h"

@implementation RewardMobWebController {
    SFSafariViewController *svc;
}

-(void)openSafariViewControllerFor:(NSString *) urlStr {
    
    // Check if the App is installed and if so, open the URL & return.
    if([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"rewardmob://"]]) {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:urlStr] options:[NSDictionary dictionary] completionHandler:nil];
        return;
    }
    
    // (Re)create the SafariViewController
    NSURL *url = [NSURL URLWithString:urlStr];
    svc = [[SFSafariViewController alloc] initWithURL:url];
    
    // Presentation & Transition Styles
    svc.modalPresentationStyle = UIModalPresentationOverFullScreen;
    svc.modalTransitionStyle = UIModalTransitionStyleCoverVertical;
    
    // Get Unity's Root ViewController & Presnt svc
    UIViewController *rootVC = [UIApplication sharedApplication].keyWindow.rootViewController;
    [rootVC presentViewController:svc animated:YES completion:nil];
}

-(void)hideSafariViewController {
    // If set, dismiss the SafariViewController.
    if(svc) {
        [svc dismissViewControllerAnimated:YES completion:^{
            NSLog(@"Hid the SafariViewController via hideSafariViewController()");
        }];
    }
}

-(void)safariViewController:(SFSafariViewController *)controller didCompleteInitialLoad:(BOOL)didLoadSuccessfully {
    /*
     Delegate is only assigned when logging out user.
     After the initial load is complete (user logout),
     then dismiss the VC. */
    [svc dismissViewControllerAnimated:YES completion:nil];
}

-(void)logoutUserForMode:(NSString *)mode {
    
    if([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"rewardmob://"]]) {
        // Authentication happens within the RewardMob app, so don't
        // bother triggering the webview in this case.
        return;
    }
    
    // (Re)create the SafariViewController
    NSString *urlStr = @"https://rewardmob.com/auth/logout";
    if([mode isEqualToString:@"dev"]) {
        urlStr = @"https://dev.rewardmob.com/auth/logout";
    }
    
    NSURL *url = [NSURL URLWithString:urlStr];
    svc = [[SFSafariViewController alloc] initWithURL:url];
    svc.delegate = self; // Asign delegate as self to know when initial load is complete.
    
    // Presentation & Transition Styles
    svc.modalPresentationStyle = UIModalPresentationOverFullScreen;
    svc.modalTransitionStyle = UIModalTransitionStyleCoverVertical;
    
    // Get Unity's Root ViewController & Presnt svc
    UIViewController *rootVC = [UIApplication sharedApplication].keyWindow.rootViewController;
    [rootVC presentViewController:svc animated:YES completion:nil];
}
@end

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

RewardMobWebController *rmwc = nil;

extern "C" {
    void _OpenSafari (const char* url) {
        if(rmwc == nil) {
            rmwc = [[RewardMobWebController alloc] init];
        }
        
        [rmwc openSafariViewControllerFor:CreateNSString(url)];
    }
    
    void _CloseSafari () {
        if(rmwc == nil) {
            rmwc = [[RewardMobWebController alloc] init];
        }
        
        [rmwc hideSafariViewController];
    }
    
    void _LogoutUser(const char* mode) {
        if(rmwc == nil) {
            rmwc = [[RewardMobWebController alloc] init];
        }
        
        [rmwc logoutUserForMode:CreateNSString(mode)];
    }
}

