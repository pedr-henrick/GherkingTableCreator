Feature:

Scenario:
	Given I am on the homepage
		| DataCriacao | Pessoa_Idade | Pessoa_ListaTarefas1_Ajudantes1_Função_Nome | Pessoa_ListaTarefas1_Ajudantes1_Função_QuantidadeExpetienciaAno | Pessoa_ListaTarefas1_Ajudantes1_Idade | Pessoa_ListaTarefas1_Ajudantes1_Nome | Pessoa_ListaTarefas1_Data | Pessoa_ListaTarefas1_Descricao | Pessoa_ListaTarefas2_Ajudantes1_Idade | Pessoa_ListaTarefas2_Ajudantes1_Nome | Pessoa_ListaTarefas2_Data | Pessoa_ListaTarefas2_Descricao | Pessoa_ListaTarefas3_Ajudantes | Pessoa_ListaTarefas3_Data | Pessoa_ListaTarefas3_Descricao | Pessoa_Nome |
		| 2024-02-24  | 23           | Seguranca                                   | 12                                                              | 12                                    | Lila                                 | 2024-02-25                | Criar GherkinTableCreator      | 25                                    | Jhonatan                             | 2024-02-24                | Despedida do Jeff              |                                | 2024-02-24                | Lavar os pratos                | Pedro       |
		| 2024-02-25  | 22           | Cliente                                     | 12                                                              | 12                                    | Lila                                 | Cliente                   | 11                             | 12                                    | Costelinha                           | 2024-02-25                | Dar banho nos cachorros        | Yasmin                         |
		When I click on the "Sign Up" button
	Then I should be taken to the sign up page
