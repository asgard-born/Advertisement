//
//  LUCVarBooleanTableViewCell.m
//
//  Lunar Unity Mobile Console
//  https://github.com/SpaceMadness/lunar-unity-console
//
//  Copyright 2019 Alex Lementuev, SpaceMadness.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

#import "Lunar-Full.h"

#import "LUCVarBooleanTableViewCell.h"

@interface LUCVarBooleanTableViewCell ()

@property (nonatomic, weak) IBOutlet UISwitch * toggleSwitch;

@end

@implementation LUCVarBooleanTableViewCell

#pragma mark -
#pragma mark Variable

- (void)setupVariable:(LUCVar *)variable
{
    [super setupVariable:variable];
    _toggleSwitch.on = [variable.value isEqualToString:@"1"];
}

#pragma mark -
#pragma mark Actions

- (IBAction)onToggleSwitch:(id)sender
{
    [self setVariableValue:_toggleSwitch.isOn ? @"1" : @"0"];
}

@end
