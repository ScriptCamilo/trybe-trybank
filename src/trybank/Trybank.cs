namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;

    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;

    public TrybankLib()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    private void ValidateAccount(int number, int agency)
    {
        try
        {
            for (int i = 0; i <= registeredAccounts; i++)
            {
                if (Bank[i, 0] == number && Bank[i, 1] == agency)
                {
                    throw new ArgumentException("A conta já está sendo usada!");
                }
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private void AddAccount(int number, int agency, int pass)
    {
        var nextAccountIndex = registeredAccounts;

        Bank[nextAccountIndex, 0] = number;
        Bank[nextAccountIndex, 1] = agency;
        Bank[nextAccountIndex, 2] = pass;
        Bank[nextAccountIndex, 3] = 0;
        registeredAccounts++;
    }

    private void ValidateUserNotLogged()
    {
        try
        {
            if (!Logged)
            {
                throw new AccessViolationException("Usuário não está logado");
            }
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        ValidateAccount(number, agency);

        AddAccount(number, agency, pass);
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        try
        {
            var foundAccount = false;
            for (int i = 0; i <= registeredAccounts; i++)
            {
                if (Bank[i, 0] == number && Bank[i, 1] == agency)
                {
                    foundAccount = true;
                    if (Bank[i, 2] == pass)
                    {
                        if (Logged == true && loggedUser == i)
                        {
                            throw new AccessViolationException("Usuário já está logado");
                        }

                        loggedUser = i;
                        Logged = true;
                        break;
                    }
                    else
                    {
                        throw new ArgumentException("Senha incorreta");
                    }
                }
            }

            if (!foundAccount)
            {
                throw new ArgumentException("Agência + Conta não encontrada");
            }
        }
        catch (AccessViolationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        ValidateUserNotLogged();

        Logged = false;
        loggedUser = -99;
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        ValidateUserNotLogged();

        return Bank[loggedUser, 3];
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        ValidateUserNotLogged();

        Bank[loggedUser, 3] += value;
    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        var balance = CheckBalance() - value;

        try
        {
            if (balance < 0)
            {
                throw new InvalidOperationException("Saldo insuficiente");
            }

            Bank[loggedUser, 3] = balance;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        Withdraw(value);

        for (int i = 0; i <= registeredAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                Bank[i, 3] += value;
            }
        }
    }
}
