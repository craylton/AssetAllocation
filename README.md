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
