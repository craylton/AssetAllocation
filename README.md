## Description ##
To diversify or not to diversify? The age-old question of how much to invest in each asset class has no right answer since everyone has their own attitude to risk. This repo attempts to make it a bit easier.

This project allows you to specify your goals in fairly simple terms, and it will tell you how much to invest in each asset class. If you specify that you want to make as much money as possible, it will tell you to put all of your cash into the riskiest asset you can find. If you specify that you simply want to avoid losing money, then it will tell you to put your cash into the safest asset possible.

However, this project gets interesting when you start to _combine_ these goals, in which case you'll be instructed to diversify to some extent. So for example, you may want to maximise your chance of making a 20% return, while also trying to avoid making a catastrophic loss. This project will advise you accordingly.

## Example ##
Try setting the following investment goals in InvestmentDistributionConsole.Program.cs:

```
InvestmentGoals investmentGoals = new(
    [
        new(1.00, 2),
        new(1.04, 3),
        new(1.10, 1),
    ],
    2);
```
This sets up 3 investment goals. One goal is to get a return of more than 0% (in other words to _not lose money_), another goal is to get more than a 4% return, and finally to get more than a 10% return.

The second parameter in each goal is the importance of that goal. Getting more than 4% ROI is 3 times more important than getting more than 10%, which I guess would just be a nice to have here.

## Development ##
I'm still treating this repo as a bit of a sandbox so this readme might become out of date. But I do plan on making a simple and fast library out of this eventually 🤞
