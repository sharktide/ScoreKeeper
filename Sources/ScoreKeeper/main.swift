import SwiftUI

struct PersonScore: Identifiable {
    let id = UUID()
    var name: String
    var score: Int
}

struct ContentView: View {
    @State private var people: [PersonScore] = [
        PersonScore(name: "Player 1", score: 0),
        PersonScore(name: "Player 2", score: 0)
    ]
    @State private var selectedNumber = ""
    
    var body: some View {
        NavigationView {
            VStack {
                TextField("Enter score change", text: $selectedNumber)
                    .keyboardType(.numberPad)
                    .padding()
                    .background(Color.gray.opacity(0.2))
                    .cornerRadius(8)
                    .padding()
                
                List {
                    ForEach(people.indices, id: \.self) { index in
                        HStack {
                            Text(people[index].name)
                            Spacer()
                            Text("\(people[index].score)")
                            Stepper("", onIncrement: {
                                if let value = Int(selectedNumber) {
                                    people[index].score += value
                                }
                            }, onDecrement: {
                                if let value = Int(selectedNumber) {
                                    people[index].score -= value
                                }
                            })
                            .labelsHidden()
                        }
                    }
                }
                
                Button("Add Player") {
                    let nextNumber = people.count + 1
                    people.append(PersonScore(name: "Player \(nextNumber)", score: 0))
                }
                .padding()
            }
            .navigationTitle("Score Keeper")
        }
    }
}

#Preview {
    ContentView()
}
