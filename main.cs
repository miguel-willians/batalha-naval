using System;

class Program
{
    static string[,] tabela = new string[11, 11]; // Tamanho do tabuleiro alterado para 10x10
    static int cont = 0;
    static int contdebarcos = 5; // Número de navios ajustado para o novo tabuleiro
    static int jogadasRestantes = 25; // Número fixo de tentativas agora é 25
    static int jogadasExtras = 0; // Jogadas extras ganhas por acertos

    static void Main()
    {
        Introducao(); // Exibe as instruções antes de começar o jogo
        InicializarTabuleiro();
        ColocarNavios();
        Jogo();
    }

    // Função de introdução com regras, número de navios e a legenda
    static void Introducao()
    {
        Console.WriteLine("Bem-vindo ao Jogo de Batalha Naval!");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Regras:");
        Console.WriteLine("1. O objetivo do jogo é afundar todos os navios do inimigo.");
        Console.WriteLine("2. Você tem 5 navios espalhados no tabuleiro.");
        Console.WriteLine("3. Cada vez que você acertar um navio, ele será marcado como 'U'.");
        Console.WriteLine("4. Cada vez que você errar, a tentativa será marcada como 'O'.");
        Console.WriteLine("5. Você deve adivinhar as coordenadas (linha e coluna) onde os navios estão localizados.");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Número de tentativas fixas: 25");
        Console.WriteLine("\nLegenda:");
        Console.WriteLine(" ~  = Água (não sabemos se há navio aqui)");
        Console.WriteLine(" O  = Tentativa falhada (não há navio aqui)");
        Console.WriteLine(" U  = Acerto (o jogador acertou um navio)");
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("Pressione Enter para começar...");
        Console.ReadLine(); // Aguarda o jogador pressionar Enter para iniciar
    }

    static void InicializarTabuleiro()
    {
        // Inicializa o tabuleiro com "~" para água
        for (int i = 0; i < 11; i++) // Usando 11 para incluir a linha e coluna 0
        {
            for (int j = 0; j < 11; j++) // Usando 11 para incluir a linha e coluna 0
            {
                tabela[i, j] = "~"; // ~ representa água (lugar vazio)
            }
        }
    }

    static void ColocarNavios()
    {
        Random rnd = new Random();
        int num1, num2;

        // Gerar o número correto de barcos
        for (int i = 0; i < contdebarcos; i++)
        {
            do
            {
                num1 = rnd.Next(1, 11); // Gera um número aleatório entre 1 e 10 para um tabuleiro 10x10
                num2 = rnd.Next(1, 11); // Gera um número aleatório entre 1 e 10 para um tabuleiro 10x10
            }
            while (tabela[num1, num2] == "X"); // Evitar colocar navios na mesma posição

            tabela[num1, num2] = "X"; // Coloca um barco na tabela
        }
    }

    static void ExibirTabuleiro()
    {
        Console.Clear();
        Console.WriteLine("Tabuleiro de Batalha Naval:");
        Console.Write("   ");
        
        // Imprimir os números das colunas (de 1 a 10)
        for (int i = 1; i <= 10; i++)
        {
            Console.Write($"{i,3}"); // Largura de 3 para alinhar os números das colunas
        }
        Console.WriteLine();

        for (int i = 1; i <= 10; i++) // Começa de 1 para não exibir a linha 0
        {
            Console.Write($"{i,3}|"); // Imprime a linha (com largura de 3 para alinhamento)
            for (int j = 1; j <= 10; j++) // Começa de 1 para não exibir a coluna 0
            {
                // Exibe o tabuleiro com o status atual
                if (tabela[i, j] == "X")
                    Console.Write(" ~ "); // Navios são escondidos
                else
                    Console.Write($" {tabela[i, j]} "); // Exibe "O" para tentativas e "U" para acertos
            }
            Console.WriteLine();
        }
    }

    static void Jogo()
    {
        int col, lin;

        // Início do jogo
        do
        {
            ExibirTabuleiro(); // Exibe o tabuleiro após cada jogada

            Console.WriteLine($"\nJogadas restantes: {jogadasRestantes + jogadasExtras}");
            col = PedirCoordenada("coluna");
            lin = PedirCoordenada("linha");

            // Verificar se a casa já foi tentada anteriormente
            if (tabela[col, lin] == "O" || tabela[col, lin] == "U")
            {
                Console.WriteLine("Esta posição já foi tentada. Tente outra.");
                continue; // Retorna ao início da rodada
            }

            // Se houver jogadas extras, ele pode jogar mais uma vez
            if (tabela[col, lin] == "X")
            {
                contdebarcos--;
                tabela[col, lin] = "U"; // Marca como acertado
                jogadasExtras++; // Concede uma jogada extra por acertar
                Console.WriteLine("------ACERTOU------\n ------Faltam {0} submarinos------", contdebarcos);
            }
            else
            {
                tabela[col, lin] = "O"; // Marca como tentado, mas falhou
                Console.WriteLine("------FALHOU------\n ------Faltam {0} submarinos------", contdebarcos);
            }

            jogadasRestantes--; // Decrementa as jogadas restantes após cada tentativa

        } while (contdebarcos > 0 && jogadasRestantes > 0); // Continua até que os navios sejam afundados ou as jogadas se esgotem

        // Exibe mensagem final
        if (contdebarcos == 0)
        {
            ExibirTabuleiro();
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------Parabéns, Você Venceu!---------------------");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("-----------------------------Tentativas: {0}--------------------------------", cont);
        }
        else
        {
            ExibirTabuleiro();
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------Fim de jogo!------------------------------");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Você não conseguiu afundar todos os navios!");
        }
    }

    static int PedirCoordenada(string tipo)
    {
        int coordenada;

        // Loop para garantir que a coordenada seja válida
        while (true)
        {
            Console.WriteLine($"Introduza a {tipo} (1-10):"); // Modificado para começar de 1-10

            // Verifica se a entrada é um número válido
            if (int.TryParse(Console.ReadLine(), out coordenada))
            {
                // Verifica se o número está dentro do intervalo permitido (1-10)
                if (coordenada >= 1 && coordenada <= 10)
                {
                    return coordenada;
                }
                else
                {
                    Console.WriteLine($"Valor inválido! A {tipo} deve estar entre 1 e 10.");
                }
            }
            else
            {
                Console.WriteLine($"Entrada inválida! Por favor, insira um número entre 1 e 10 para a {tipo}.");
            }
        }
    }
}
