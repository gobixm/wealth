# Work breakdown structure

1. [ ] Common Libs
   1. [x] Study Dapper
   2. [x] Unit of Work + Repo pattern
   3. [ ] Moex index response Parser
      1. [x] Securities parser (http://iss.moex.com/iss/reference/5)
      2. [x] Index response Parser
      3. [ ] Price response Parser
   4. [x] Choose REST client
   5. [ ] Moex Data Provider
      1. [x] Securities
      2. [ ] Current Price (last) for securities
      3. [x] Index SecIds and rates
2. [ ] Data access layer
   1. [x] Deploy postgresql
   2. [ ] Currency Repository
   3. [ ] Ticker Repository
   4. [ ] Transaction History Repository
   5. [ ] Index Repository
   6. [ ] Seed data (currency)
3. [ ] Frontend prototype
   1. [ ] Scaffold
   2. [ ] Deploy in docker
4. [ ] Add ticker scenario
   1. [ ] Logic + presenter
   2. [ ] UI
5. [ ] Add Transaction scenario
   1. [ ] Logic + presenter
   2. [ ] Ui
6. [ ] Calculate current index analogue scenarion
   1. [ ] Logic + presenter
   2. [ ] Ui
7. [ ] Propose investments to taget index scenario
   1. [ ] Amount calculation logic optimization based on Price
   2. [ ] Logic + presenter
   3. [ ] Ui
