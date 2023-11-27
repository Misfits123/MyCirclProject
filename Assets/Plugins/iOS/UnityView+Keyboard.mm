#import <GameController/GameController.h> // FIX
#import "UnityView.h"
#include "UI/Keyboard.h"
#include <sys/time.h>
#include <map>
#include <vector>

static NSArray* keyboardCommands = nil;

extern "C" int UnityGetAppleTVRemoteAllowExitToMenu();
extern "C" void UnitySetAppleTVRemoteAllowExitToMenu(int val);

@implementation UnityView (Keyboard)

// Keyboard shortcuts don't provide events for key up
// Keyboard shortcut callbacks are called with 0.4 (first time) and 0.1 (following times) seconds interval while pressing the key
// Below we implement key expiration mechanism where key up event is generated if shortcut callback
// is not called for specific key for more than <kKeyTimeoutInSeconds>

typedef std::map<int, double> KeyMap;
static const double kKeyTimeoutInSeconds = 0.5;
GCKeyboard * _coalescedKeyboard; // FIX

static KeyMap& GetKeyMap()
{
    static KeyMap s_Map;
    return s_Map;
}

static double GetTimeInSeconds()
{
    timeval now;
    gettimeofday(&now, NULL);

    return now.tv_sec + now.tv_usec / 1000000.0;
}

- (void)createKeyboard
{
    // only English keyboard layout is supported
    NSString* baseLayout = @"1234567890-=qwertyuiop[]asdfghjkl;'\\`zxcvbnm,./!@#$%^&*()_+{}:\"|<>?~ \t\r\b\\";
    NSString* numpadLayout = @"1234567890-=*+/.\r";
    NSString* upperCaseLetters = @"QWERTYUIOPASDFGHJKLZXCVBNM";
    NSString* shortcutCharacters = @"axcv";

    size_t sizeOfKeyboardCommands = baseLayout.length + numpadLayout.length + upperCaseLetters.length + 11;
    NSMutableArray* commands = [NSMutableArray arrayWithCapacity: sizeOfKeyboardCommands];

    void (^addKey)(NSString *keyName, UIKeyModifierFlags modifierFlags) = ^(NSString *keyName, UIKeyModifierFlags modifierFlags)
    {
        UIKeyCommand* command = [UIKeyCommand keyCommandWithInput: keyName modifierFlags: modifierFlags action: @selector(handleCommand:)];
        if (@available(iOS 15.0, *))
            command.wantsPriorityOverSystemBehavior = YES;
        [commands addObject: command];
    };

    for (NSInteger i = 0; i < baseLayout.length; ++i)
    {
        NSString* input = [baseLayout substringWithRange: NSMakeRange(i, 1)];
        addKey(input, kNilOptions);
    }
    for (NSInteger i = 0; i < shortcutCharacters.length; ++i)
    {
        NSString* input = [shortcutCharacters substringWithRange: NSMakeRange(i, 1)];
        [commands addObject: [UIKeyCommand keyCommandWithInput: input modifierFlags: UIKeyModifierCommand action: @selector(handleCommand:)]];
    }
    for (NSInteger i = 0; i < numpadLayout.length; ++i)
    {
        NSString* input = [numpadLayout substringWithRange: NSMakeRange(i, 1)];
        addKey(input, UIKeyModifierNumericPad);
    }
    for (NSInteger i = 0; i < upperCaseLetters.length; ++i)
    {
        NSString* input = [upperCaseLetters substringWithRange: NSMakeRange(i, 1)];
        addKey(input, UIKeyModifierShift);
    }

    // pageUp, pageDown
    addKey(UIKeyInputPageUp, kNilOptions);
    addKey(UIKeyInputPageDown, kNilOptions);

    // up, down, left, right, esc
    addKey(UIKeyInputUpArrow, kNilOptions);
    addKey(UIKeyInputDownArrow, kNilOptions);
    addKey(UIKeyInputLeftArrow, kNilOptions);
    addKey(UIKeyInputRightArrow, kNilOptions);
    addKey(UIKeyInputEscape, kNilOptions);

    // caps Lock, shift, control, option, command
    addKey(@"", UIKeyModifierAlphaShift);
    addKey(@"", UIKeyModifierShift);
    addKey(@"", UIKeyModifierControl);
    addKey(@"", UIKeyModifierAlternate);
    addKey(@"", UIKeyModifierCommand);

    keyboardCommands = commands.copy;
}

- (void)addKeyboardListener // FIX
{
	_coalescedKeyboard = nil;
    _coalescedKeyboard = GCKeyboard.coalescedKeyboard;
    _coalescedKeyboard.keyboardInput.keyChangedHandler = ^(GCKeyboardInput *keyboard, GCControllerButtonInput *key, GCKeyCode keyCode, BOOL pressed) {
        int code = 0;
		if (keyCode == GCKeyCodeDeleteOrBackspace)
            code = 8;
        else if (keyCode == GCKeyCodeTab)
            code = 9;
        else if (keyCode == GCKeyCodeReturnOrEnter)
            code = 13;
        else if (keyCode == GCKeyCodeEscape)
            code = 27;
        else if (keyCode == GCKeyCodeSpacebar)
            code = 32;
        else if (keyCode == GCKeyCodeQuote)
            code = 39;
        else if (keyCode == GCKeyCodeComma)
            code = 44;
        else if (keyCode == GCKeyCodeHyphen)
            code = 45;
        else if (keyCode == GCKeyCodePeriod)
            code = 46;
        else if (keyCode == GCKeyCodeSlash)
            code = 47;
        else if (keyCode == GCKeyCodeZero)
            code = 48;
        else if (keyCode == GCKeyCodeOne)
            code = 49;
        else if (keyCode == GCKeyCodeTwo)
            code = 50;
        else if (keyCode == GCKeyCodeThree)
            code = 51;
        else if (keyCode == GCKeyCodeFour)
            code = 52;
        else if (keyCode == GCKeyCodeFive)
            code = 53;
        else if (keyCode == GCKeyCodeSix)
            code = 54;
        else if (keyCode == GCKeyCodeSeven)
            code = 55;
        else if (keyCode == GCKeyCodeEight)
            code = 56;
        else if (keyCode == GCKeyCodeNine)
            code = 57;
        else if (keyCode == GCKeyCodeSemicolon)
            code = 59;
        else if (keyCode == GCKeyCodeEqualSign)
            code = 61;
        else if (keyCode == GCKeyCodeOpenBracket)
            code = 91;
        else if (keyCode == GCKeyCodeBackslash)
            code = 92;
        else if (keyCode == GCKeyCodeCloseBracket)
            code = 93;
        else if (keyCode == GCKeyCodeGraveAccentAndTilde)
            code = 96;
        else if (keyCode == GCKeyCodeKeyA)
            code = 97;
        else if (keyCode == GCKeyCodeKeyB)
            code = 98;
        else if (keyCode == GCKeyCodeKeyC)
            code = 99;
        else if (keyCode == GCKeyCodeKeyD)
            code = 100;
        else if (keyCode == GCKeyCodeKeyE)
            code = 101;
        else if (keyCode == GCKeyCodeKeyF)
            code = 102;
        else if (keyCode == GCKeyCodeKeyG)
            code = 103;
        else if (keyCode == GCKeyCodeKeyH)
            code = 104;
        else if (keyCode == GCKeyCodeKeyI)
            code = 105;
        else if (keyCode == GCKeyCodeKeyJ)
            code = 106;
        else if (keyCode == GCKeyCodeKeyK)
            code = 107;
        else if (keyCode == GCKeyCodeKeyL)
            code = 108;
        else if (keyCode == GCKeyCodeKeyM)
            code = 109;
        else if (keyCode == GCKeyCodeKeyN)
            code = 110;
        else if (keyCode == GCKeyCodeKeyO)
            code = 111;
        else if (keyCode == GCKeyCodeKeyP)
            code = 112;
        else if (keyCode == GCKeyCodeKeyQ)
            code = 113;
        else if (keyCode == GCKeyCodeKeyR)
            code = 114;
        else if (keyCode == GCKeyCodeKeyS)
            code = 115;
        else if (keyCode == GCKeyCodeKeyT)
            code = 116;
        else if (keyCode == GCKeyCodeKeyU)
            code = 117;
        else if (keyCode == GCKeyCodeKeyV)
            code = 118;
        else if (keyCode == GCKeyCodeKeyW)
            code = 119;
        else if (keyCode == GCKeyCodeKeyX)
            code = 120;
        else if (keyCode == GCKeyCodeKeyY)
            code = 121;
        else if (keyCode == GCKeyCodeKeyZ)
            code = 122;	
        else if (keyCode == GCKeyCodeUpArrow)
            code = 273;
        else if (keyCode == GCKeyCodeDownArrow)
            code = 274;
        else if (keyCode == GCKeyCodeRightArrow)
            code = 275;
        else if (keyCode == GCKeyCodeLeftArrow)
            code = 276;
        else if (keyCode == GCKeyCodePageUp)
            code = 280;
        else if (keyCode == GCKeyCodePageDown)
            code = 281;
        else if (keyCode == GCKeyCodeF1)
            code = 282;
        else if (keyCode == GCKeyCodeF2)
            code = 283;
        else if (keyCode == GCKeyCodeF3)
            code = 284;
        else if (keyCode == GCKeyCodeF4)
            code = 285;
        else if (keyCode == GCKeyCodeF5)
            code = 286;
        else if (keyCode == GCKeyCodeF6)
            code = 287;
        else if (keyCode == GCKeyCodeF7)
            code = 288;
        else if (keyCode == GCKeyCodeF8)
            code = 289;
        else if (keyCode == GCKeyCodeF9)
            code = 290;
        else if (keyCode == GCKeyCodeF10)
            code = 291;
        else if (keyCode == GCKeyCodeF11)
            code = 292;
        else if (keyCode == GCKeyCodeF12)
            code = 293;
        else if (keyCode == GCKeyCodeF13)
            code = 294;
        else if (keyCode == GCKeyCodeF14)
            code = 295;
        else if (keyCode == GCKeyCodeF15)
            code = 296;
        else if (keyCode == GCKeyCodeCapsLock)
            code = 301;
        else if (keyCode == GCKeyCodeLeftShift)
            code = 304;
        else if (keyCode == GCKeyCodeRightShift)
            code = 304;
        else if (keyCode == GCKeyCodeLeftControl)
            code = 306;
        else if (keyCode == GCKeyCodeRightControl)
            code = 306;
        else if (keyCode == GCKeyCodeLeftAlt)
            code = 308;
        else if (keyCode == GCKeyCodeRightAlt)
            code = 308;
        else if (keyCode == GCKeyCodeLeftGUI)
            code = 310;
        else if (keyCode == GCKeyCodeRightGUI)
            code = 310;
        
        if (code != 0)
            UnitySetKeyboardKeyState(code, pressed);
    };
}

- (NSArray*)keyCommands
{
	[self addKeyboardListener];
	
    //keyCommands take control of buttons over UITextView, that's why need to return nil if text input field is active or we have an external keyboard attached AND a first responder
    if ([[KeyboardDelegate Instance] status] == Visible || ([[KeyboardDelegate Instance] hasExternalKeyboard] && [self hasFirstResponderInHeirachy: UnityGetGLView()]))
        return nil;

    if (keyboardCommands == nil)
    {
        [self createKeyboard];
    }
    return keyboardCommands;
}

- (bool)hasFirstResponderInHeirachy:(UIView*)view
{
    if (view.isFirstResponder)
        return true;

    for (UIView* subview in view.subviews)
    {
        if ([self hasFirstResponderInHeirachy: subview])
            return true;
    }

    return false;
}

- (bool)isValidCodeForButton:(int)code
{
    return (code > 0 && code < 128);
}

- (void)handleCommand:(UIKeyCommand *)command
{
//    NSString* input = command.input;
//    UIKeyModifierFlags modifierFlags = command.modifierFlags;
//
//    char inputChar = ([input length] == 1) ? [input characterAtIndex: 0] : 0;
//    int code = (int)inputChar; // ASCII code
//
//    if (![self isValidCodeForButton: code])
//    {
//        code = 0;
//    }
//
//    if ((modifierFlags & UIKeyModifierAlphaShift) != 0)
//        code = UnityStringToKey("caps lock");
//    if ((modifierFlags & UIKeyModifierShift) != 0)
//        code = UnityStringToKey("left shift");
//    if ((modifierFlags & UIKeyModifierControl) != 0)
//        code = UnityStringToKey("left ctrl");
//    if ((modifierFlags & UIKeyModifierAlternate) != 0)
//        code = UnityStringToKey("left alt");
//    if ((modifierFlags & UIKeyModifierCommand) != 0)
//        code = UnityStringToKey("left cmd");
//
//    if ((modifierFlags & UIKeyModifierNumericPad) != 0)
//    {
//        switch (inputChar)
//        {
//            case '0':
//                code = UnityStringToKey("[0]");
//                break;
//            case '1':
//                code = UnityStringToKey("[1]");
//                break;
//            case '2':
//                code = UnityStringToKey("[2]");
//                break;
//            case '3':
//                code = UnityStringToKey("[3]");
//                break;
//            case '4':
//                code = UnityStringToKey("[4]");
//                break;
//            case '5':
//                code = UnityStringToKey("[5]");
//                break;
//            case '6':
//                code = UnityStringToKey("[6]");
//                break;
//            case '7':
//                code = UnityStringToKey("[7]");
//                break;
//            case '8':
//                code = UnityStringToKey("[8]");
//                break;
//            case '9':
//                code = UnityStringToKey("[9]");
//                break;
//            case '-':
//                code = UnityStringToKey("[-]");
//                break;
//            case '=':
//                code = UnityStringToKey("equals");
//                break;
//            case '*':
//                code = UnityStringToKey("[*]");
//                break;
//            case '+':
//                code = UnityStringToKey("[+]");
//                break;
//            case '/':
//                code = UnityStringToKey("[/]");
//                break;
//            case '.':
//                code = UnityStringToKey("[.]");
//                break;
//            case '\r':
//                code = UnityStringToKey("enter");
//                break;
//            default:
//                break;
//        }
//    }
//
//    if (input == UIKeyInputUpArrow)
//        code = UnityStringToKey("up");
//    else if (input == UIKeyInputDownArrow)
//        code = UnityStringToKey("down");
//    else if (input == UIKeyInputRightArrow)
//        code = UnityStringToKey("right");
//    else if (input == UIKeyInputLeftArrow)
//        code = UnityStringToKey("left");
//    else if (input == UIKeyInputEscape)
//        code = UnityStringToKey("escape");
//    else if ([input isEqualToString: @"UIKeyInputPageUp"])
//        code = UnityStringToKey("page up");
//    else if ([input isEqualToString: @"UIKeyInputPageDown"])
//        code = UnityStringToKey("page down");

    // UnitySendKeyboardCommand(command, code);

//    KeyMap::iterator item = GetKeyMap().find(code);
//    if (item == GetKeyMap().end())
//    {
//        // New key is down, register it and its time
//        // UnitySetKeyboardKeyState(code, true);
//        GetKeyMap()[code] = GetTimeInSeconds();
//    }
//    else
//    {
//        // Still holding the key, update its time
//        item->second = GetTimeInSeconds();
//    }
}

- (void)processKeyboard
{
//    KeyMap& map = GetKeyMap();
//    if (map.size() == 0)
//        return;
//    std::vector<int> keysToUnpress;
//    double nowTime = GetTimeInSeconds();
//    for (KeyMap::iterator item = map.begin();
//         item != map.end();
//         item++)
//    {
//        // Key has expired, register it for key up event
//        if (nowTime - item->second > kKeyTimeoutInSeconds)
//            keysToUnpress.push_back(item->first);
//    }
//
//    for (std::vector<int>::iterator item = keysToUnpress.begin();
//         item != keysToUnpress.end();
//         item++)
//    {
//        map.erase(*item);
//        // UnitySetKeyboardKeyState(*item, false);
//    }
}

@end
